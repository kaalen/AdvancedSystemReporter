namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Data.Items;

  /// <summary>
  /// Filters items based on a minimum number of versions. Number of versions 
  /// is passed in as parameter.
  /// </summary>
  public class MultipleVersions : NumberFilter
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
      Item itemElement = element as Item;
      if (itemElement != null)
      {
        return itemElement.Versions.Count >= this.Number;
      }
      return false;
    }
  }
}