using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
  using System;
  using Sitecore.StringExtensions;

  /// <summary>
  /// Filters items which have no workflow or no workflow state but should have according to their template's standard values.
  /// </summary>
  public class HasWorkflow : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    private const string WorkflowFieldName = "__workflow";
    private const string DefaultWorkflowFieldName = "__default workflow";

    /// <summary>
    /// Executes the filter which returns true for items which do not have a workflow or workflow state but 
    /// they should according to their data template's standard values.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns></returns>
    public override bool Filter(object element)
    {
      Item item = element as Item;
      if (item != null)
      {
        if (!String.IsNullOrEmpty(item[WorkflowFieldName]) || !String.IsNullOrEmpty(item[DefaultWorkflowFieldName]))
        {
          return true;
        }
      }
      return false;
    }
  }
}