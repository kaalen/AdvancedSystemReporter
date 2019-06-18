using System;
using System.Linq;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Workflows;
using Sitecore.Data.Items;
using Sitecore.Feature.Reports.DisplayItems;
using Sitecore.Security.Accounts;

namespace Sitecore.Feature.Reports.Viewers
{
  /// <summary>
  /// Implements a report viewer which enables presentation of content item’s 
  /// current workflow state details such as event date and time, event name, 
  /// workflow, workflow state, workflow state duration, user who triggered 
  /// last event and user who rejected content item.
  /// </summary>
  public class WorkflowStateViewer : BaseViewer
  {
    #region Private members

    private User _draftUser;
    private Item _newWorkflowState;
    private User _rejectUser;
    private User _user;
    private Item _workflowState = null;

    #endregion

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
          string lcColumn = column.ToLower();
          string text = GetColumnText(lcColumn, Columns[i].Parameters);
          if (string.IsNullOrEmpty(text))
          {
            dElement.AddColumn(header, Entry.Item[lcColumn]);
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
    /// <returns></returns>
    private string GetColumnText(string lcColumn, string parameters)
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
          return WorkflowStateName;
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
          return GetPublishedDate(Entry.Item);
        default:
          return Sitecore.Feature.Reports.Fields.FieldHelper.GetFriendlyFieldValue(lcColumn, parameters, Entry.Item);
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
          IWorkflow workflow = Entry.Item.State.GetWorkflow();
          if (workflow != null)
          {
            var events = from wEvent in workflow.GetHistory(Entry.Item)
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
          IWorkflow workflow = Entry.Item.State.GetWorkflow();
          if (workflow != null)
          {
            var events = from wEvent in workflow.GetHistory(Entry.Item)
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
    protected Item WorkflowState
    {
      get
      {
        if (_workflowState == null && Entry != null)
        {
          _workflowState = Entry.WorkflowEvent.NewState == string.Empty ? null : Db.GetItem(Entry.WorkflowEvent.NewState);
        }
        return _workflowState;
      }
    }

    /// <summary>
    /// Gets the name of the workflow state.
    /// </summary>
    /// <value>The name of the workflow state.</value>
    protected string WorkflowStateName
    {
      get
      {
        return (WorkflowState == null) ? String.Empty : WorkflowState.Name;
      }
    }

    /// <summary>
    /// Gets the new state of the workflow.
    /// </summary>
    /// <value>The new state of the workflow.</value>
    protected Item NewWorkflowState
    {
      get
      {
        if (_newWorkflowState == null && Entry != null)
        {
          _newWorkflowState = Sitecore.Context.ContentDatabase.GetItem(Entry.WorkflowEvent.NewState);
        }
        return _newWorkflowState;
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
        return (NewWorkflowState == null) ? string.Empty : NewWorkflowState.Name;
      }
    }

    /// <summary>
    /// Gets the workflow.
    /// </summary>
    /// <value>The workflow.</value>
    protected Item Workflow
    {
      get
      {
        return (WorkflowState == null) ? null : WorkflowState.Parent;
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
        return Workflow == null ? string.Empty : Workflow.Name;
      }
    }

    /// <summary>
    /// Gets the name of the event.
    /// </summary>
    /// <param name="entry">The entry.</param>
    /// <returns></returns>
    protected string GetEventName(ItemWorkflowEvent entry)
    {
      if (WorkflowState != null)
      {
        //retrive state's commands
        foreach (Item command in WorkflowState.GetChildren())
        {
          //match command whose new state matches event's new state
          if (command["next state"] == Entry.WorkflowEvent.NewState)
          {
            return command.Name;
          }
        }
        if (entry.WorkflowEvent.OldState == WorkflowStates.QAReview && Entry.WorkflowEvent.NewState == WorkflowStates.Approved)
        {
          return "Accept";
        }
        if ((entry.WorkflowEvent.OldState == WorkflowStates.AwaitingUpdate || entry.WorkflowEvent.OldState == WorkflowStates.AwaitingReview) && Entry.WorkflowEvent.NewState == WorkflowStates.Draft)
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
        IWorkflow workflow = Entry.Item.Database.WorkflowProvider.GetWorkflow(Entry.Item);
        if (workflow != null && WorkflowState != null)
        {
          var followingEvents = from e in workflow.GetHistory(Entry.Item)
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

    protected string GetPublishedDate(Item item)
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
          return publishDate.ToString("g");
        }
      }
      return string.Empty;
    }
  }
}