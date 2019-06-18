using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
	/// <summary>
	/// Filters items based on field values they contains.
	/// </summary>
	class FieldDoesNotContainValue : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		public const string FIELD_NAME_PARAMETER = "FieldName";
		public const string FIELD_VALUE_PARAMETER = "Value";

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		/// <value>The name of the field.</value>
		public string FieldName
		{
			get
			{
				return this.GetParameter(FIELD_NAME_PARAMETER);
			}
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get
			{
				return this.GetParameter(FIELD_VALUE_PARAMETER);
			}
		}

		public override bool Filter(object element)
		{
			Item item = null;
			switch (element.GetType().FullName)
			{
				case "Sitecore.Feature.Reports.DisplayItems.ItemWorkflowEvent":
					item = element is ItemWorkflowEvent ? (element as ItemWorkflowEvent).Item : null;
					break;
				case "Sitecore.Feature.Reports.DisplayItems.MediaUsageItem":
					item = element is MediaUsageItem ? (element as MediaUsageItem).Item : null;
					break;
				default:
					item = element as Item;
					break;
			}

			if (item != null)
			{
				Field field = item.Fields[FieldName];
				foreach (string value in Value.Split(','))
				{
					if (field != null && field.Value.Contains(value))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
