using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
  /// <summary>
  /// Filters items which have security permissions applied.
  /// </summary>
  public class HasSecurity : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    private string _userOrRole = null;

    public string UserOrRole
    {
      get
      {
        return _userOrRole;
      }
      set
      {
        _userOrRole = value;
      }
    }

    public override bool Filter(object element)
    {
      Item item = element as Item;
      if (item != null)
      {
        if (!string.IsNullOrEmpty(UserOrRole))
        {
          return (item["__security"].Contains(UserOrRole));
        }
        return (item["__security"] != string.Empty);
      }
      return false;
    }
  }
}