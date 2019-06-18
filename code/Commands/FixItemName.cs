using System;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command attempts to rename item to give it a valid name.
	/// </summary>
	class FixItemName : Command
	{
		public override void Execute(CommandContext context)
		{
			foreach (Item item in context.Items)
			{
				try
				{
					string proposedName = this.ProposeValidItemName(item.Name, item.Name);
					string message = string.Format("Item will be renamed from {0} to {1}.",
						item.Name,
						proposedName);

					Log.Info(message, this);
					using (new EditContext(item))
					{
						item.Name = proposedName;
					};
				}
				catch (Exception ex)
				{
					SheerResponse.ShowError(ex);
				}
			}
		}

		private string ProposeValidItemName(string name, string defaultValue)
		{
			Assert.ArgumentNotNull(name, "name");
			if (ItemUtil.IsItemNameValid(name))
			{
				return name;
			}
			//encode to ASCII
			name = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(name));

			//replace invalid characters with their replacements
			name = MainUtil.DecodeName(name);

			foreach (char ch in Sitecore.Configuration.Settings.InvalidItemNameChars)
			{
				name = name.Replace(ch.ToString(), " ");
			}
			name = name.Trim();
			if (ItemUtil.IsItemNameValid(name))
			{
				return name;
			}
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				if (char.IsLetterOrDigit(c))
				{
					builder.Append(c);
				}
				else if (char.IsWhiteSpace(c))
				{
					builder.Append(" ");
				}
			}
			name = builder.ToString().Trim();
			if (ItemUtil.IsItemNameValid(name))
			{
				return name;
			}
			Log.Warn(string.Format("Cannot create a valid item name from string '{0}'", name), this);
			if (!ItemUtil.IsItemNameValid(defaultValue))
			{
				throw new Exception("Cannot create a valid item name. Please check the related setting in web.config file");
			}
			return defaultValue;
		}
	}
}
