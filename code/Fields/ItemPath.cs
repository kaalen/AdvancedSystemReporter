using Sitecore.Data.Fields;

namespace Sitecore.Feature.Reports.Fields
{
	/// <summary>
	/// Implements a custom field which is used to represent item’s path.
	/// </summary>
	class ItemPath : CustomField
	{
		public ItemPath(Field innerField)
			: base(innerField)
		{
		}

		public string Path
		{
			get
			{
				return base.InnerField.Item.Paths.Path;
			}
		}
	}
}
