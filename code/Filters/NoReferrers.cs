using Sitecore.Data.Items;
using Sitecore.Links;

namespace Sitecore.Feature.Reports.Filters
{
	class NoReferrers : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		/// <summary>
		/// Filters the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public override bool Filter(object element)
		{
			Item item = element as Item;
			if (item != null)
			{
				ItemLink[] links = Sitecore.Globals.LinkDatabase.GetReferrers(item);
				if (links.Length == 0)
				{
					return true;
				}
			}
			return false;
		}

    //todo: invert parameter
	}
}
