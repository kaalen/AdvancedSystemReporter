using System;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Workflows;

namespace Sitecore.Feature.Reports.Viewers
{
  /// <summary>
  /// Implements a report viewer which enables presentation of details for 
  /// content items which were rejected during workflow. It enables display 
  /// of rejection date, rejection user id, rejection user’s full name, 
  /// rejection user’s business unit and rejection comment.
  /// </summary>
  class RejectedViewer : DetailedViewer
  {
    protected override string GetColumnText(string name, Item itemElement)
    {
      switch (name)
      {
        case "rejection date":
          return FormatDateTime(GetRejectionEvent(itemElement).Date);
        case "rejection user id":
          return GetRejectionUser(itemElement).Profile.UserName;
        case "rejection user fullname":
          return GetRejectionUser(itemElement).Profile.FullName;
        case "rejection user organisation":
          return GetRejectionUser(itemElement).Profile.GetCustomProperty("Business unit");
        case "rejection comment":
          return GetRejectionEvent(itemElement).CommentFields["Comments"];
        default:
          return base.GetColumnText(name, itemElement);
      }
    }

    private User _rejectionUser;
    /// <summary>
    /// Gets the rejection user.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    private User GetRejectionUser(Item item)
    {
      if (_rejectionUser == null)
      {
        _rejectionUser = Sitecore.Security.Accounts.User.FromName(GetRejectionEvent(item).User, false);
      }
      return _rejectionUser;
    }

    private WorkflowEvent _rejectionEvent = null;
    /// <summary>
    /// Gets the last rejection event.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    private WorkflowEvent GetRejectionEvent(Item item)
    {
      if (_rejectionEvent == null)
      {
        //if item in workflow
        IWorkflow workflow = item.State.GetWorkflow();
        if (workflow != null)
        {
          //retrieve workflow history for current version
          var rejections = from e in workflow.GetHistory(item)
                           where ((e.OldState == "{B3D9B273-660C-4698-BEE7-145BCDD0C039}" || e.OldState == "{3FFDCB17-1512-4476-983E-CA998997C9ED}") && e.NewState == "{E45963E2-C6EC-4002-AC41-82077E1ADAD0}")
                           orderby e.Date descending
                           select e;
          //if item was rejected, return false 
          if (rejections.Count() > 0)
          {
            return rejections.First();
          }
        }
      }
      return _rejectionEvent;
    }

    /// <summary>
    /// Formats the date time.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    protected string FormatDateTime(DateTime dateTime)
    {
      if (dateTime != DateTime.MinValue)
      {
        return dateTime.ToString("dd/MM/yyyy HH:mm");
      }
      return string.Empty;
    }
  }
}
