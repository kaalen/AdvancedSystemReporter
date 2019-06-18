using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command which applies default workflow and places item into a default 
	/// workflow state as specified by the item's template standard values.
	/// </summary>
	public class ApplyWorkflow : Command
	{
		/// <summary>
		/// Executes the command which applies default workflow and places item into a default workflow state 
		/// as specified by the item's template standard values.
		/// </summary>
        /// <param name="context">The context.</param>
		public override void Execute(CommandContext context)
		{
			foreach (Item item in context.Items)
			{
				if (item.Template.StandardValues != null)
				{
					Item tsv = item.Template.StandardValues;
					//grab default workflow
					ReferenceField defaultWorkflowField = tsv.Fields["__default workflow"];
					if (defaultWorkflowField != null && defaultWorkflowField.TargetItem != null)
					{
						Item workflow = defaultWorkflowField.TargetItem;
						item.Editing.BeginEdit();
						item["__workflow"] = workflow.ID.ToString();
						item["__workflow state"] = workflow["initial state"];
						item.Editing.EndEdit();
					}
				}
			}
		}
	}
}
