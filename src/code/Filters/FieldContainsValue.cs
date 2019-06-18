using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
  using System;

  /// <summary>
  /// Filters items based on field values they contains.
  /// </summary>
  class FieldContainsValue : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    public const string FIELD_NAME_PARAMETER = "FieldName";
    public const string VALUE_PARAMETER = "Value";

    public string FieldName
    {
      get
      {
        return base.GetParameter(FIELD_NAME_PARAMETER);
      }
    }

    public bool IsCaseSensitive { get; set; }

    public string Value
    {
      get
      {
        return base.GetParameter(VALUE_PARAMETER);
      }
    }

    public override bool Filter(object element)
    {
      Item item = null;
      switch (element.GetType().FullName)
      {
        case "Sitecore.Feature.Reports.DisplayItems.ItemWorkflowEvent":
          item = element is ItemWorkflowEvent ? (element as ItemWorkflowEvent).Item : null;
          break;
        case "Sitecore.Feature.Reports.DisplayItems.MediaUsageItem":
          item = element is MediaUsageItem ? (element as MediaUsageItem).Item : null;
          break;
        default:
          item = element as Item;
          break;
      }

      if (item != null)
      {
        Field field = item.Fields[FieldName];
        if (field == null)
          return false;

        foreach (string value in Value.Split(','))
        {
          if (field.Value.Contains(value) || (!IsCaseSensitive && field.Value.ToLower().Contains(value.ToLower())))
          {
            return true;
          }
        }
      }
      return false;
    }
  }
}
