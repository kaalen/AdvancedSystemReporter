using System;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Fields
{
	/// <summary>
	/// Helper class which provides method that return friendly field values, 
	/// e.g. for a field of type Sitecore.Feature.Reports.Fields.User it returns value passed 
	/// in by the Field parameter.
	/// </summary>
	class FieldHelper
	{
		public static string GetFriendlyFieldValue(string name, string parameters, Item itemElement)
		{
			Field field = itemElement.Fields[name];
			if (field != null)
			{
				if (parameters != null)
				{
					{
						string type = StringUtil.ExtractParameter("Type", parameters);
						switch (type)
						{
							case "Sitecore.Feature.Reports.Fields.User":
								User user = new User(field);
								string property = StringUtil.ExtractParameter("Field", parameters);
								return user.GetType().GetProperty(property).GetValue(user, null).ToString();
							default:
								break;
						}
					}
				}
				else
				{
					return itemElement[name];
				}
			}
			return String.Empty;
		}

	}
}
