namespace Sitecore.Feature.Reports.DisplayItems
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Workflows;

  public class ItemWorkflowEvent
  {
    public const string DefaultCommentFieldName = "Comments";

    public ItemWorkflowEvent(Item item, WorkflowEvent workflowEvent)
    {
      this.Item = item;
      this.WorkflowEvent = workflowEvent;
    }

    public WorkflowEvent WorkflowEvent { get; private set; }
    public Item Item { get; private set; }

    public string Comments
    {
      get
      {
        return WorkflowEvent?.CommentFields[DefaultCommentFieldName];
      }
    }
  }
}