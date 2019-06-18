using System;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Collections;
using System.Collections.Generic;
using Sitecore.Feature.Reports.DomainObjects;
using System.Xml;

namespace Sitecore.Feature.Reports.Interface
{
  public abstract class BaseViewer : BaseReportObject
  {
    #region Abstract methods

    public abstract void Display(DisplayElement dElement);

    #endregion

    public string DateFormat { get; set; }

    public string GetDateFormat(string defaultFormat)
    {
      return StringUtil.GetString(DateFormat, defaultFormat, "dd/MM/yyyy HH:mm:ss");
    }

    private static BaseViewer Create(string type)
    {
      return BaseReportObject.CreateObject(type) as BaseViewer;
    }

    internal static BaseViewer Create(string type, string parameters)
    {
      Assert.ArgumentNotNull(type, "type");
      Assert.ArgumentNotNull(parameters, "parameters");

      BaseViewer oViewer = BaseViewer.Create(type);
      Assert.ArgumentNotNull(oViewer, $"Could not create a viewer ({type}). Check if the viewer is implemented.");

      oViewer.AddParameters(parameters);
      return oViewer;
    }

    internal static BaseViewer Create(string type, string parameters, string columnsXml)
    {
      Assert.ArgumentNotNull(type, "type");
      Assert.ArgumentNotNull(parameters, "parameters");

      BaseViewer oViewer = BaseViewer.Create(type);
      Assert.ArgumentNotNull(oViewer, $"Could not create a viewer ({type}). Check if the viewer is implemented.");

      oViewer.AddParameters(parameters);
      InitializeColumns(oViewer, columnsXml);
      //backwards compatibility
      if (oViewer.Columns.Count == 0)
      {
        InitializeColumnsOld(oViewer);
      }
      return oViewer;
    }

    private static void InitializeColumnsOld(BaseViewer baseViewer)
    {
      var columns = baseViewer.GetParameter("columns");
      if (!string.IsNullOrEmpty(columns))
      {
        var cols = columns.Split(',');
        foreach (var col in cols)
        {
          var c = new Column()
          {
            Name = col,
            Header = col
          };
          baseViewer.Columns.Add(c);
        }
      }
    }

    private static void InitializeColumns(BaseViewer oViewer, string columnsXml)
    {
      oViewer.Columns = new List<Column>();
      if (!string.IsNullOrEmpty(columnsXml))
      {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(columnsXml);
        XmlNodeList columnNodes = doc.DocumentElement.SelectNodes("Column");
        for (int i = 0; i < columnNodes.Count; i++)
        {
          Column column = new Column
          {
            Name = columnNodes[i].Attributes["name"].Value,
            Header = columnNodes[i].InnerText
          };
          oViewer.Columns.Add(column);
        }
      }
    }

    /// <summary>
    /// Gets the columns.
    /// </summary>
    /// <value>The columns.</value>
    public List<Column> Columns { get; protected set; }

    public virtual string[] AvailableColumns
    {
      get
      {
        return new string[] { };
      }
    }
  }
}