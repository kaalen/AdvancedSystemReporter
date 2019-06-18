using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command changes media item's template to equivalent versioned template, 
	/// e.g. from unversioned Doc to versioned Doc.
	/// </summary>
	class ChangeToVersionedTemplate : Command
	{
		public override void Execute(CommandContext context)
		{
			foreach (Item item in context.Items)
			{
				//check if item is unversioned media type
				if (item.Template.FullName.StartsWith("System/Media/Unversioned"))
				{
					//find versionable template
					string versionableTemplateName = item.Template.FullName.Replace("Unversioned", "Versioned");
					Sitecore.Data.Database db = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;
					TemplateItem newTemplate = db.GetTemplate(versionableTemplateName);
					//change template 
					item.ChangeTemplate(newTemplate);
				}
			}
			Sitecore.Context.ClientPage.SendMessage(this, "reports:run");
		}
	}
}
