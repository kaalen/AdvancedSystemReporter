using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Data.Items;
  using Sitecore.Feature.Reports.Interface;

  public class DueDateFilter : BaseFilter
  {

    public int DueInFilter { get; set; }

    public override bool Filter(object element)
    {
      Item item = element as Item;
      if (item == null) return false;

      var targetDateField = (Sitecore.Data.Fields.DateField)item.Fields[TargetDateField];
      if (targetDateField == null || !targetDateField.InnerField.HasValue)
      {
        return false;
      }

      var duration = targetDateField.DateTime.Subtract(DateTime.Now);

      return duration.Days < MaxNumber;
    }

    public int MaxNumber { get; set; }

    public string TargetDateField { get; set; }
  }
}