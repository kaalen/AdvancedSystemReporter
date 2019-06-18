using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.DomainObjects
{
    
    public class FilterItem:ReferenceItem
    {
        public FilterItem(Item innerItem) : base(innerItem)
        {
        }
    }
}
