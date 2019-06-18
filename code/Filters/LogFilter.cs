using System;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Feature.Reports.Logs;

  /// <summary>
  /// Filters log entries whose age is less than given parameter.
  /// </summary>
  public class LogFilter : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    public int Age
    {
      get; set;
    }
    public override bool Filter(object element)
    {
      LogItem li = element as LogItem;

      if (li == null || Age < 0)
      {
        return true;
      }

      return DateTime.Now.CompareTo(li.DateTime.AddHours(Age)) < 0;
    }
  }
}
