using Sitecore.Data.Fields;

namespace Sitecore.Feature.Reports.Fields
{
	/// <summary>
	/// Implements a custom reference field which returns empty string if 
	/// target item is null.
	/// </summary>
	class ReferenceField : Sitecore.Data.Fields.ReferenceField
	{
		public ReferenceField(Field innerField)
			: base(innerField)
		{ }

		public string TargetItemName
		{
			get
			{
				if (TargetItem == null)
				{
					return string.Empty;
				}
				return TargetItem.Name;
			}
		}
	}
}
