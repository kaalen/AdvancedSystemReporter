using System;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
	public class ManyVersions : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		/// <summary>
		/// Number of versions, 1 by default
		/// </summary>
		private int numberOfVersions = 1;

		/// <summary>
		/// Gets or sets the number of versions.
		/// </summary>
		/// <value>The number of versions.</value>
		public string NumberOfVersions
		{
			set
			{
				int parameter;
				Int32.TryParse(value, out parameter);
				numberOfVersions = Math.Max(1, parameter);
			}
		}

		public override bool Filter(object element)
		{
			if (element is Item)
			{
				Item item = element as Item;
				return ((item != null) && (item.Versions.Count > numberOfVersions));
			}
			return false;
		}
	}
}

