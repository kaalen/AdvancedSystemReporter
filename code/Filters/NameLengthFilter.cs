namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Data.Items;

  /// <summary>
  /// Filters items base on item name’s length. Filter eliminates items which 
  /// do not match desired name length.
  /// </summary>
  public class NameLengthFilter : Sitecore.Feature.Reports.Interface.BaseFilter
  {
    public int MinNameLength { get; set; }

    public override bool Filter(object element)
    {
      Item itemElement = element as Item;
      if (itemElement != null)
      {
        Sitecore.Diagnostics.Assert.ArgumentNotNull(element, "element");
        return itemElement.Name.Length > this.MinNameLength;
      }
      return false;
    }
  }
}