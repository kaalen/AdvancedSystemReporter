using System;
using System.Linq;
using Sitecore.Common;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Feature.Reports.Fields;
using Sitecore.Feature.Reports.Filters;
using Sitecore.Workflows;

namespace Sitecore.Feature.Reports.Viewers
{
  using Sitecore.Feature.Reports.Extensions;
  using Sitecore.Links;

  /// <summary>
  /// Implements a report viewer which enables presentation of item details 
  /// such as content owner name, content owner organisational unit, days to 
  /// review, review group, workflow, workflow state, workflow state duration, 
  /// publish date and time and unpublish date and time.
  /// </summary>
  public class DetailedViewer : ItemViewer
  {
    protected Item Item;

    protected override string GetColumnText(string name, Item itemElement)
    {
      Item = itemElement;
      switch (name)
      {
        case "content owner name":
          return ContentOwner == null ? string.Empty : ContentOwner.FullName;
        case "content owner organisational unit":
          return ContentOwner == null ? string.Empty : ContentOwner.OrganisationalUnit;
        case "days to review":
          return NumberOfDaysTillReview;
        case "review group":
          return ReviewGroup;
        case "workflow":
          return WorkflowName;
        case "workflow state":
          return WorkflowStateName;
        case "workflow state duration":
          return DurationInWorkflowState;
        case "publish date time":
          return GetPublishDateTime("g");
        case "publish date":
          return GetPublishDateTime("dd/MM/yyyy");
        case "publish time":
          return GetPublishDateTime("HH:mm");
        case "unpublish date":
          return GetUnpublishDateTime("dd/MM/yyyy");
        case "unpublish time":
          return GetUnpublishDateTime("HH:mm");
        case "url":
          if (itemElement.Visualization.GetLayout(Sitecore.Context.Device) != null)
          {
            return itemElement.Url(new UrlOptions { Site = itemElement.GetContextSite() });
          }
          return null;
        default:
          //todo: refactor this
          //if (String.IsNullOrEmpty(parameters))
          //{
          //  return base.GetColumnText(name, itemElement);
          //}
          //else
          //{
          //  return FieldHelper.GetFriendlyFieldValue(name, itemElement);
          //}
          return FieldHelper.GetFriendlyFieldValue(name, null, itemElement);
      }
    }

    private User _contentOwner;

    /// <summary>
    /// Gets the content owner.
    /// </summary>
    /// <value>The content owner.</value>
    public User ContentOwner
    {
      get
      {
        if (_contentOwner == null && Item.Fields["content owner"] != null)
        {
          _contentOwner = new User(Item.Fields["content owner"]);
        }
        return _contentOwner;
      }
    }

    protected string GetPublishDateTime(string format)
    {
      DateTimeRange visibleFrom = UnpublishedBetween.GetRange(Item);

      if (visibleFrom.From == DateTime.MinValue || visibleFrom == DateTimeRange.Empty)
      {
        return String.Empty;
      }
      return visibleFrom.From.ToString(format);
    }

    protected string GetUnpublishDateTime(string format)
    {
      DateTimeRange visibleRange = UnpublishedBetween.GetRange(Item);

      if (visibleRange.To == DateTime.MaxValue || visibleRange == DateTimeRange.Empty)
      {
        return String.Empty;
      }
      return visibleRange.To.ToString(format);
    }

    /// <summary>
    /// Gets the number of days till review.
    /// </summary>
    /// <param name="itemElement">The item element.</param>
    /// <returns></returns>
    protected string NumberOfDaysTillReview
    {
      get
      {
        DateField reviewDate = Item.Fields["review date"];
        if (reviewDate != null && reviewDate.DateTime != DateTime.MinValue)
        {
          return reviewDate.DateTime.Subtract(DateTime.Today).Days.ToString();
        }
        else
        {
          return string.Empty;
        }
      }
    }

    /// <summary>
    /// Gets the review group.
    /// </summary>
    /// <param name="itemElement">The item element.</param>
    /// <returns></returns>
    protected string ReviewGroup
    {
      get
      {
        string daysTillReviewStr = NumberOfDaysTillReview;
        int daysTillReview;
        if (Int32.TryParse(daysTillReviewStr, out daysTillReview))
        {
          if (daysTillReview == 15)
          {
            return "15 days";
          }
          else if (daysTillReview == 3)
          {
            return "3 days";
          }
          else if (daysTillReview == 1)
          {
            return "1 day";
          }
          else if (daysTillReview == 0)
          {
            return "0 days";
          }
          else if (daysTillReview < 0)
          {
            return "overdue";
          }
        }
        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the duration the item has spent in the current workflow state.
    /// </summary>
    /// <param name="itemElement">The item element.</param>
    /// <returns></returns>
    protected string DurationInWorkflowState
    {
      get
      {
        if (Workflow != null)
        {
          var enumerator = from e in Workflow.GetHistory(Item)
                           orderby e.Date descending
                           select e;
          if (enumerator.Count() > 0)
          {
            WorkflowEvent lastEvent = enumerator.First();
            if (lastEvent.NewState == WorkflowState.StateID)
            {
              TimeSpan span = DateTime.Now.Subtract(lastEvent.Date);
              return String.Format("{0} days {1} hours {2} minutes", span.Days, span.Hours, span.Minutes);
            }
          }
        }
        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the workflow state.
    /// </summary>
    /// <param name="itemElement">The item element.</param>
    /// <returns></returns>
    protected string WorkflowStateName
    {
      get
      {
        return (WorkflowState == null) ? string.Empty : WorkflowState.DisplayName;
      }
    }

    protected Sitecore.Workflows.WorkflowState WorkflowState
    {
      get
      {
        return (Workflow == null) ? null : Workflow.GetState(Item);
      }
    }

    /// <summary>
    /// Gets the name of the workflow.
    /// </summary>
    /// <param name="itemElement">The item element.</param>
    /// <returns></returns>
    protected string WorkflowName
    {
      get
      {
        if (Workflow != null)
        {
          return Workflow.Appearance.DisplayName;
        }
        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the workflow.
    /// </summary>
    /// <value>The workflow.</value>
    protected IWorkflow Workflow
    {
      get
      {
        if (_workflow == null)
        {
          _workflow = Item.Database.WorkflowProvider.GetWorkflow(Item);
        }
        return _workflow;
      }
    }

    private IWorkflow _workflow = null;
  }
}