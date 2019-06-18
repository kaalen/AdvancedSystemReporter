using System;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
	class NoPublishableVersions : BaseFilter
	{
		public override bool Filter(object element)
		{
			Item item = element as Item;
			if (item == null)
			{
				return false;
			}
			Item validVersion=item.Publishing.GetValidVersion(DateTime.Now, true, false);
			return validVersion == null;
		}
	}
}
