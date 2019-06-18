using System.Text.RegularExpressions;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Viewers
{
  using Sitecore.Feature.Reports.Logs;

  /// <summary>
  /// Implements a report viewer which enables presentation of user license 
  /// details. It enables presentation of details from log files, such as log 
  /// date and time, username, full name, email, business unit, time when user 
  /// session commenced.
  /// </summary>
  internal class UserLicenseViewer : BaseViewer
  {
    public Sitecore.Security.Accounts.User User { get; set; }

    public LogItem Entry { get; set; }

    public override void Display(DisplayElement dElement)
    {
      Entry = dElement.Element as LogItem;
      if (Entry != null)
      {
        dElement.Value = Entry.ToString();

        for (int i = 0; i < Columns.Count; i++)
        {
          string column = Columns[i].Name;
          string header = Columns[i].Header;
          string text = GetColumnText(column, Entry);
          dElement.AddColumn(header, text);
        }
      }
    }

    private string GetColumnText(string lcColumn, LogItem Entry)
    {
      switch (lcColumn)
      {
        case "log date":
          return Entry.DateTime.ToShortDateString();
        case "log time":
          return Entry.DateTime.ToShortTimeString();
        case "log datetime":
          return Entry.DateTime.ToString("g");
        case "username":
          Regex usernameRegex = new Regex("Username:(.*?) ");
          Match matchUsername = usernameRegex.Match(Entry.Message);
          if (matchUsername.Success && matchUsername.Groups.Count > 1)
          {
            string username = matchUsername.Groups[1].Value;
            User = Sitecore.Security.Accounts.User.FromName(username, true);
            return username;
          }
          break;
        case "full name":
          return User == null ? string.Empty : User.Profile.FullName;
        case "email":
          return User == null ? string.Empty : User.Profile.Email;
        case "business unit":
          return User == null ? string.Empty : User.Profile.GetCustomProperty("Business unit");
        case "time comenced":
          Regex createdRegex = new Regex("Created:(.*?)\n");
          Match matchCreated = createdRegex.Match(Entry.Message);
          if (matchCreated.Success && matchCreated.Groups.Count > 1)
          {
            return matchCreated.Groups[1].Value;
          }
          break;
      }
      return string.Empty;
    }
  }
}