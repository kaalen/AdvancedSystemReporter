using System.Text;
using System.Web.Security;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Feature.Reports.DomainObjects;
using Sitecore.Security.Accounts;

namespace Sitecore.Feature.Reports.Viewers
{
  /// <summary>
  /// Implements a report viewer which enables presentation of user details 
  /// such as username, full name, email, business unit, comment, user status, 
  /// date created, date modified, user roles and domains.
  /// </summary>
  public class DetailedUserViewer : BaseViewer
  {
    public override void Display(DisplayElement dElement)
    {
      for (int i = 0; i < Columns.Count; i++)
      {
        Column column = Columns[i];

        string text = GetColumnText(column.Name, column.Parameters, dElement);
        dElement.AddColumn(column.Header, text);
      }
    }

    private static string GetColumnText(string name, string parameters, DisplayElement dElement)
    {
      User user = dElement.Element as User;
      MembershipUser membershipUser = Membership.GetUser(user.Name);

      switch (name)
      {
        case "username":
          return removeDomain(user.Name);
        case "fullname":
          return user.Profile.FullName;
        case "email":
          return user.Profile.Email;
        case "business unit":
          return user.Profile.GetCustomProperty("Business unit");
        case "comment":
          return user.Profile.Comment;
        case "user status":
          return user.Profile.State;
        case "date created":
          return membershipUser.CreationDate.ToString("dd/MM/yyyy HH:mm");
        case "date modified":
          return user.Profile.LastUpdatedDate.ToString("dd/MM/yyyy HH:mm");
        case "user roles":
          return getRolesList(user);
        case "domains":
          return user.Profile.ManagedDomainNames;
        default:
          return string.Empty;
      }
    }

    /// <summary>
    /// Gets the roles list.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns></returns>
    private static string getRolesList(User element)
    {
      StringBuilder roleList = new StringBuilder();
      foreach (var role in element.Roles)
      {
        roleList.AppendFormat("{0}, ", removeDomain(role.DisplayName));
      }
      return roleList.ToString().TrimEnd(',', ' ');
    }

    private static string removeDomain(string userOrRoleName)
    {
      int index = userOrRoleName.IndexOf('\\');
      if (index > -1)
      {
        userOrRoleName = userOrRoleName.Remove(0, index + 1);
      }
      return userOrRoleName;
    }
  }
}