using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.DomainObjects
{
    
    public class ValueItem:BaseItem
    {
        public ValueItem(Item innerItem) : base(innerItem)
        {
        }
        
        public string Value
        {
            get { return InnerItem["value"]; }
        }
    }
}
