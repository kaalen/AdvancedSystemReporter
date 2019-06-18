using System;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command resets icon field.
	/// </summary>
	class ResetIconField : Command
	{
		public override void Execute(CommandContext context)
		{
			foreach (Item item in context.Items)
			{
				string[] fieldNames = Fields.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (string fieldName in fieldNames)
				{
					try
					{
						using (new EditContext(item))
						{
							item.Fields[fieldName].Reset();
						}
					}
					catch (Exception)
					{
						string errorMsg = string.Format("An error has occured while resetting field {0} on item {1} ({2})", fieldName, item.Name, item.ID);
						SheerResponse.ShowError(errorMsg, "");
					}
				}
			}
		}

		public string Fields { get; set; }
	}
}
