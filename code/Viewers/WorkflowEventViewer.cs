using System;
using System.Linq;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Workflows;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Data.Fields;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Viewers
{
  using Sitecore.StringExtensions;

  /// <summary>
  /// Implements a viewer which enables presentation of ItemWorkflowEvent 
  /// objects. It enables display of details such as event date and time, 
  /// event name, workflow name, new workflow state name, workflow state 
  /// duration, item’s details, user who triggered the event, draft user’s 
  /// details and reject user’s details.
  /// </summary>
  public class WorkflowEventViewer : BaseViewer
  {
    private User _draftUser;
    private User _rejectUser;
    private User _user;
    private Item _workflowState;

    public override void Display(DisplayElement dElement)
    {
      Entry = dElement.Element as ItemWorkflowEvent;
      if (Entry != null)
      {
        dElement.Value = Entry.Item.Uri.ToString();
        dElement.Header = Entry.Item.Name;
        dElement.Icon = Entry.Item.Appearance.Icon;

        for (int i = 0; i < Columns.Count; i++)
        {
          string column = Columns[i].Name;
          string header = Columns[i].Header;
          string parameters = Columns[i].Parameters;
          string text = GetColumnText(column, Entry.Item, parameters);
          if (string.IsNullOrEmpty(text))
          {
            dElement.AddColumn(header, Entry.Item[column]);
          }
          else
          {
            dElement.AddColumn(header, text);
          }
        }
      }
    }

    /// <summary>
    /// Gets the column text.
    /// </summary>
    /// <param name="lcColumn">The lc column.</param>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    private string GetColumnText(string lcColumn, Item item, string parameters)
    {
      switch (lcColumn)
      {
        case "event date":
          return Entry.WorkflowEvent.Date.ToString("dd/MM/yyyy");
        case "event time":
          return Entry.WorkflowEvent.Date.ToString("hh:mm");
        case "event date time":
          return Entry.WorkflowEvent.Date.ToString("g");
        case "event name":
          return GetEventName(Entry);
        case "workflow name":
          return WorkflowName;
        case "workflow state":
          return this.OldWorkflowStateName;
        case "new workflow state":
          return NewWorkflowStateName;
        case "workflow state duration":
          return DurationInWorkflowState;
        case "content item name":
          return Entry.Item.Name;
        case "content item version":
          return Entry.Item.Version.ToString();
        case "path":
        case "content item path":
          return Entry.Item.Paths.ContentPath;
        case "content item businesss owner":
          return WorkflowName;
        case "user triggered":
          return Entry.WorkflowEvent.User;
        case "user triggered name":
          return EventUserFullName;
        case "user triggered business unit":
          return EventUserBusinessUnit;
        case "comment":
          return Entry.Comments;
        case "draft user":
          return (DraftUser == null) ? string.Empty : DraftUser.Name;
        case "draft user name":
          return (DraftUser == null) ? string.Empty : DraftUser.Profile.FullName;
        case "draft user business unit":
          return (DraftUser == null) ? string.Empty : DraftUser.Profile.GetCustomProperty("Business unit");
        case "reject user":
          return (RejectUser == null) ? string.Empty : RejectUser.Name;
        case "reject user name":
          return (RejectUser == null) ? string.Empty : RejectUser.Profile.FullName;
        case "reject user business unit":
          return (RejectUser == null) ? string.Empty : RejectUser.Profile.GetCustomProperty("Business unit");
        case "content owner name":
          return ContentOwner == null ? string.Empty : ContentOwner.Profile.FullName;
        case "content owner business unit":
          return ContentOwner == null ? string.Empty : ContentOwner.Profile.GetCustomProperty("Business unit");
        case "content item published":
          return GetContentItemPublished(Entry.Item);
        default:
          Field field = Entry.Item.Fields[lcColumn];
          if (field != null)
          {
            if (String.IsNullOrEmpty(parameters))
            {
              return field.Value;
            }
            else
            {
              string type = StringUtil.ExtractParameter("Type", parameters);
              switch (type)
              {
                case "Sitecore.Feature.Reports.Fields.User":
                  Sitecore.Feature.Reports.Fields.User user = new Sitecore.Feature.Reports.Fields.User(field);
                  string property = StringUtil.ExtractParameter("Field", parameters);
                  return user.GetType().GetProperty(property).GetValue(user, null).ToString();
                default:
                  break;
              }
            }
          }
          return String.Empty;
      }
    }

    /// <summary>
    /// Gets the content owner.
    /// </summary>
    /// <value>The content owner.</value>
    protected User ContentOwner
    {
      get
      {
        string contentOwnerID = Entry.Item["content owner"];
        if (!String.IsNullOrEmpty(contentOwnerID))
        {
          return User.FromName(contentOwnerID, false);
        }
        return null;
      }
    }

    /// <summary>
    /// Gets the draft user.
    /// </summary>
    /// <value>The draft user.</value>
    protected User DraftUser
    {
      get
      {
        if (_draftUser == null)
        {
          if (Workflow != null)
          {
            var events = from wEvent in Workflow.GetHistory(Entry.Item)
              where (wEvent.OldState == WorkflowStates.Draft) ||
                    (wEvent.OldState == string.Empty && wEvent.NewState == WorkflowStates.Draft)
              orderby wEvent.Date descending
              select wEvent;
            _draftUser = (events.Count() > 0) ? User.FromName(events.First().User, false) : null;
          }
        }
        return _draftUser;
      }
    }

    protected User RejectUser
    {
      get
      {
        if (_rejectUser == null)
        {
          if (Workflow != null)
          {
            var events = from wEvent in Workflow.GetHistory(Entry.Item)
              where (wEvent.OldState == WorkflowStates.QAReview || wEvent.OldState == WorkflowStates.AwaitingApproval) && //reject from QA review
                    wEvent.NewState == WorkflowStates.Draft
              orderby wEvent.Date ascending
              select wEvent;
            _rejectUser = (events.Count() == 1) ? User.FromName(events.First().User, false) : null;
          }
        }
        return _rejectUser;
      }
    }

    /// <summary>
    /// Gets the event user.
    /// </summary>
    /// <value>The event user.</value>
    protected User EventUser
    {
      get
      {
        if (_user == null)
        {
          _user = User.FromName(Entry.WorkflowEvent.User, false);
        }
        return _user;
      }
    }

    /// <summary>
    /// Gets the full name of the event user.
    /// </summary>
    /// <value>The full name of the event user.</value>
    protected string EventUserFullName
    {
      get
      {
        return EventUser.Profile.FullName;
      }
    }

    /// <summary>
    /// Gets the event user business unit.
    /// </summary>
    /// <value>The event user business unit.</value>
    protected string EventUserBusinessUnit
    {
      get
      {
        return EventUser.Profile.GetCustomProperty("Business unit");
      }
    }

    protected ItemWorkflowEvent Entry { get; set; }

    private Sitecore.Data.Database Db
    {
      get
      {
        return Entry.Item.Database;
      }
    }

    /// <summary>
    /// Gets the state of the workflow.
    /// </summary>
    /// <value>The state of the workflow.</value>
    protected Item OldWorkflowState
    {
      get
      {
        if (_workflowState == null && Entry != null)
        {
          _workflowState = Entry.WorkflowEvent.OldState == string.Empty ? null : Db.GetItem(Entry.WorkflowEvent.OldState);
        }
        return _workflowState;
      }
    }

    /// <summary>
    /// Gets the name of the workflow state.
    /// </summary>
    /// <value>The name of the workflow state.</value>
    protected string OldWorkflowStateName
    {
      get
      {
        if (!String.IsNullOrEmpty(Entry.WorkflowEvent.OldState))
        {
          return Entry.Item.State.GetWorkflow().GetState(Entry.WorkflowEvent.OldState).DisplayName;
        }
        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the name of the new workflow state.
    /// </summary>
    /// <value>The name of the new workflow state.</value>
    protected string NewWorkflowStateName
    {
      get
      {
        if (Entry.WorkflowEvent.NewState.IsNullOrEmpty())
        {
          return string.Empty;
        }
        return Entry.Item.State.GetWorkflow().GetState(Entry.WorkflowEvent.NewState).DisplayName;
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
        return Db.WorkflowProvider.GetWorkflow(Entry.Item);
      }
    }

    /// <summary>
    /// Gets the name of the workflow.
    /// </summary>
    /// <value>The name of the workflow.</value>
    protected string WorkflowName
    {
      get
      {
        return Workflow == null ? string.Empty : Workflow.Appearance.DisplayName;
      }
    }

    /// <summary>
    /// Gets the name of the event.
    /// </summary>
    /// <param name="entry">The entry.</param>
    /// <returns></returns>
    protected string GetEventName(ItemWorkflowEvent entry)
    {
      if (this.OldWorkflowState != null)
      {
        //retrive state's commands
        foreach (Item command in this.OldWorkflowState.GetChildren())
        {
          //match command whose new state matches event's new state
          if (command["next state"] == entry.WorkflowEvent.NewState)
          {
            return command.Name;
          }
        }
        if (entry.WorkflowEvent.OldState == WorkflowStates.QAReview && entry.WorkflowEvent.NewState == WorkflowStates.Approved)
        {
          return "Accept";
        }
        if ((entry.WorkflowEvent.OldState == WorkflowStates.AwaitingUpdate || entry.WorkflowEvent.OldState == WorkflowStates.AwaitingReview) && entry.WorkflowEvent.NewState == WorkflowStates.Draft)
        {
          return "Update";
        }
      }

      //unknown name
      return "?";
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
        if (Workflow != null && this.OldWorkflowState != null)
        {
          var followingEvents = from e in Workflow.GetHistory(Entry.Item)
            where e.Date > Entry.WorkflowEvent.Date
            orderby e.Date ascending
            select e;
          TimeSpan span = new TimeSpan();
          if (followingEvents.Count() > 0)
          {
            WorkflowEvent nextEvent = followingEvents.First();
            string currentStateID = Entry.WorkflowEvent.NewState;
            if (nextEvent != null && nextEvent.OldState == currentStateID)
            {
              span = nextEvent.Date.Subtract(Entry.WorkflowEvent.Date);
            }
          }

          else
          {
            //last event which occured (is still lasting)
            span = DateTime.Now.Subtract(Entry.WorkflowEvent.Date);
          }
          return String.Format("{0} days {1} hours {2} minutes", span.Days, span.Hours, span.Minutes);
        }
        return string.Empty;
      }
    }

    protected string GetContentItemPublished(Item item)
    {
      DateTime publishDate = item.Publishing.ValidFrom;
      if (!item.Publishing.HideVersion &&
          !item.Publishing.NeverPublish)
      {
        if (publishDate < item.Publishing.PublishDate)
        {
          publishDate = item.Publishing.PublishDate;
        }
        if (publishDate < item.Statistics.Created)
        {
          publishDate = item.Statistics.Created;
        }
        if (publishDate < item.Publishing.UnpublishDate &&
            publishDate < item.Publishing.ValidTo)
        {
          return publishDate.ToString("dd/MM/yyyy hh:mm");
        }
      }
      return string.Empty;
    }
  }
}