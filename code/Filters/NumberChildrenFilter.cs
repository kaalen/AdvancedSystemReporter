using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Feature.Reports.Interface;

  /// <summary>
  /// Filters items which have at least a certain Number of child items.
  /// </summary>
  public class NumberChildrenFilter : BaseFilter
  {
    public static string NUMBER_PARAMETER = "Number";
    private int _number = int.MinValue;

    protected int Number
    {
      get
      {
        if (!int.TryParse(this.GetParameter(NUMBER_PARAMETER), out this._number))
        {
          this._number = 0;
        }

        return this._number;
      }
      set
      {
        _number = value;
      }
    }

    public override bool Filter(object element)
    {
      Item item = element as Item;
      return item?.Children.Count > this.Number;
    }
  }
}