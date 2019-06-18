using System;
using System.Text.RegularExpressions;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
  internal class RegexFieldFilter : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    public const string REGEX_PARAMETER = "Regex";

    private string Field { get; set; }


    private Regex _regex;

    private Regex RegexObject
    {
      get
      {
        if (_regex == null)
        {
          string value = base.GetParameter(REGEX_PARAMETER);
          if (!String.IsNullOrEmpty(value))
          {
            _regex = new Regex(value);
          }
        }
        return _regex;
      }
    }

    public override bool Filter(object element)
    {
      Item item = element as Item;
      if (item != null)
      {
        if (string.IsNullOrEmpty(Field))
        {
          return RegexObject.IsMatch(item.Name);
        }
        else
        {
          return RegexObject.IsMatch(item[Field]);
        }
      }
      return false;
    }
  }
}