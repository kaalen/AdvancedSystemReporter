using System;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
	class ItemsWithResettableVersions : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		public override bool Filter(object element)
		{
		    var item = element as Item;
            if (item == null) return true;

				return ((item.Versions.Count == 1) && (item.Version.Number > 1)); 
			}
	}
}
