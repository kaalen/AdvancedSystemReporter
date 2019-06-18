using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports.Viewers
{
  using Sitecore.Data.Items;
  using Sitecore.Feature.Reports.Interface;

  public class DueDateViewer : BaseViewer
  {
    public const string DefaultDisplayFormat = "{0:%d} day(s)";

    public override void Display(DisplayElement dElement)
    {
      var itemElement = dElement.Element as Item;
      if (itemElement == null)
      {
        return;
      }

      var targetDateField = (Sitecore.Data.Fields.DateField)itemElement.Fields[TargetDateField];
      if (targetDateField == null || !targetDateField.InnerField.HasValue)
      {
        return;
      }

      var duration = targetDateField.DateTime.Subtract(DateTime.Now);
      var textToDisplay = String.Format(DisplayFormat ?? DefaultDisplayFormat, duration);

      dElement.AddColumn(this.DisplayColumn, textToDisplay);
    }

    public string TargetDateField { get; set; }
    public string DisplayColumn { get; set; }
    public string DisplayFormat { get; set; }
  }
}