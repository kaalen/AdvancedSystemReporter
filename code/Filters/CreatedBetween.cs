using System;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Data.Items;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
  using System.Linq;
  using Sitecore.Data;

  internal class CreatedBetween : BaseFilter
  {
    public const string FROM_DATE_PARAMETER = "FromDate";
    public const string TO_DATE_PARAMETER = "ToDate";

    /// <summary>
    /// Gets from date.
    /// </summary>
    /// <value>From date.</value>
    public DateTime FromDate
    {
      get
      {
        string value = base.GetParameter(FROM_DATE_PARAMETER);
        return Sitecore.DateUtil.ParseDateTime(value, DateTime.MinValue);
      }
    }

    /// <summary>
    /// Gets to date.
    /// </summary>
    /// <value>To date.</value>
    public DateTime ToDate
    {
      get
      {
        string value = base.GetParameter(TO_DATE_PARAMETER);
        return Sitecore.DateUtil.ParseDateTime(value, DateTime.MaxValue);
      }
    }

    /// <summary>
    /// Whether to use the first version
    /// </summary>
    /// <value>Use first version.</value>
    public bool UseFirstVersion { get; set; }

    public override bool Filter(object element)
    {
      Item item = null;
      if (element is Item)
      {
        item = element as Item;
      }
      else if (element is ItemWorkflowEvent)
      {
        item = (element as ItemWorkflowEvent).Item;
      }
      if (item != null)
      {
        if (UseFirstVersion)
        {
          var versions = item.Versions.GetVersionNumbers();
          var minVersion = versions.Min(v => v.Number);
          item = item.Database.GetItem(item.ID, item.Language, Version.Parse(minVersion.ToString()));
        }
        DateTime dateCreated = item.Statistics.Created;
        if (FromDate <= dateCreated && dateCreated < ToDate)
        {
          return true;
        }
      }
      return false;
    }
  }
}