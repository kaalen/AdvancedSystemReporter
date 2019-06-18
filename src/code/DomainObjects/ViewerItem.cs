using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.DomainObjects
{
  public class ViewerItem : ReferenceItem
  {
    public ViewerItem(Item i) : base(i)
    {
    }


    public string ColumnsXml
    {
      get
      {
        return InnerItem["columns"];
      }
    }
  }
}