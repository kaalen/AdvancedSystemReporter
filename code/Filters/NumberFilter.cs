using System;

namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Data.Items;

  public class NumberFilter : Sitecore.Feature.Reports.Interface.BaseFilter
  {

    public int MaxNumber { get; set; }
    public int MinNumber { get; set; }
    public string FieldName { get; set; }

    public override bool Filter(object element)
    {
      Item item = element as Item;
      if (item == null) return false;

      var number = 0;
      if (int.TryParse(item[this.FieldName], out number))
      {
        return MinNumber <= number && number <= MaxNumber;
      }
      return false;
    }
  }
}