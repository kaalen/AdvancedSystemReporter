using System;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command allows deleting of content item.
	/// </summary>
	class DeleteItem : Command
	{
		public override void Execute(CommandContext context)
		{
			foreach (Item item in context.Items)
			{
				try
				{
					item.Delete();
				}
				catch (Exception ex)
				{
					SheerResponse.ShowError(ex);
				}
			}
		}
	}
}
