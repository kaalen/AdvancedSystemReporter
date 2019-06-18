using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Sitecore.Diagnostics;


namespace Sitecore.Feature.Reports.Interface
{
  public abstract class BaseReportObject
  {
    public string Name { get; set; }

    private NameValueCollection parameters;

    private NameValueCollection ParseParameters(string values)
    {
      return Sitecore.StringUtil.ParseNameValueCollection(values, '|', '=');
    }

    public void AddParameters(string values)
    {
      parameters = this.ParseParameters(values);
      this.AssignProperties(values);
    }

    public bool UpdateParameters(string values)
    {
      var flag = updateParameters(values);
      if (flag)
      {
        this.AssignProperties(values);
      }
      return flag;
    }

    private bool updateParameters(string values)
    {
      if (parameters == null)
      {
        AddParameters(values);
        return true;
      }
      NameValueCollection nvc = this.ParseParameters(values);
      if (nvc.Count != parameters.Count)
      {
        AddParameters(values);
        return true;
      }

      foreach (var key in nvc.AllKeys)
      {
        if (nvc[key] != parameters[key])
        {
          parameters = nvc;
          return true;
        }
      }
      return false;
    }

    private void AssignProperties(string values)
    {
      var splitValues = values.Split('|');
      foreach (string param in splitValues)
      {
        var split = param.Split(new char[] { '=' }, 2);
        var parameterName = split[0];

        var propertyinfo = this.GetType().GetProperty(parameterName,
          BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
        if (propertyinfo?.SetMethod != null)
        {
          var parameterValue = split[1];
          Sitecore.Reflection.ReflectionUtil.SetProperty(this, propertyinfo, parameterValue);
        }
        else
        {
          Log.Warn(String.Format("Sitecore.Feature.Reports: cannot assign value to property {0} in type {1}", param, GetType()), this);
        }
      }
    }

    protected string GetParameter(string name)
    {
      return parameters?[name];
    }

    protected static object CreateObject(string type)
    {
      return Sitecore.Reflection.ReflectionUtil.CreateObject(type, new object[] { });
    }
  }
}