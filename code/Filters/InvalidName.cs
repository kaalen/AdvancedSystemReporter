using Sitecore.Data.Items;
using System.Text.RegularExpressions;

namespace Sitecore.Feature.Reports.Filters
{
	class InvalidName : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		public override bool Filter(object element)
		{
			Item item = element as Item;
			if (item != null)
			{
				string name = item.Name;
				//return !ItemUtil.IsItemNameValid(item.Name);
				if (name.Length == 0)
				{
					return true;
				}
				if (name[name.Length - 1] == '.')
				{
					return true;
				}
				if (name.Length != name.Trim().Length)
				{
					//"An item name cannot start or end with blanks."
					return true;
				}
				if (name.IndexOfAny(Sitecore.Configuration.Settings.InvalidItemNameChars) >= 0)
				{
					string str = new string(Sitecore.Configuration.Settings.InvalidItemNameChars);
					//An item name cannot contain any of the following characters: {0} (controlled by the setting InvalidItemNameChars)
					return true;
				}
				string itemNameValidation = Sitecore.Configuration.Settings.ItemNameValidation;
				if ((itemNameValidation.Length > 0) && !Regex.IsMatch(name, itemNameValidation))
				{
					//An item name must satisfy the pattern: {0} (controlled by the setting ItemNameValidation)
					return true;
				}
			}
			return false;
		}
	}
}
