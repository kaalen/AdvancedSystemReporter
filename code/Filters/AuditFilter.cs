using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
  /// <summary>
  /// Filters audit items based on entry’s user and verb.
  /// </summary>
  public class AuditFilter : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    public static string USER_PARAMETER = "user";
    public static string VERB_PARAMETER = "verb";

    public string Verb
    {
      //get { return this.GetParameter(VERB_PARAMETER); } 
      get; set;
    }
    public string User
    { //get { return this.GetParameter(USER_PARAMETER); } 
      get; set;
    }

    public override bool Filter(object element)
    {
      var auditItem = element as AuditItem;

      if (auditItem == null)
      {
        return true;
      }

      return (string.IsNullOrEmpty(Verb) || auditItem.Verb == null ||
              auditItem.Verb.Contains(Verb))
              && (string.IsNullOrEmpty(User) || auditItem.User == null ||
            auditItem.User.Equals(User, System.StringComparison.InvariantCultureIgnoreCase));
    }
  }
}
