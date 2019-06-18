using Sitecore.Data.Items;
using Sitecore.Data.Validators;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command moves items to Approved or Published workflow state (depending 
	/// on the workflow) if they are valid.
	/// </summary>
	public class ApproveCheat : Command
	{
		public override void Execute(CommandContext context)
		{
			foreach (var item in context.Items)
			{
				if (ItemPassesValidation(item))
				{
					item.Editing.BeginEdit();
					string publishContentItem = "{7F6BC961-B461-41BB-AD0F-01F6E27F6CAF}";
					string directPublish = "{0D1AD050-D11F-4726-9EE9-221B237B2C77}";
					if (item["__default workflow"] == publishContentItem) // publish content item
					{
						item["__workflow"] = publishContentItem;
						item["__workflow state"] = "{FA5F56EF-BBD8-4564-A6FD-2306F74C648D}"; //approved
					}
					else if (item["__default workflow"] == directPublish) //direct publish
					{
						item["__workflow"] = directPublish;
						item["__workflow state"] = "{460767C5-A7F1-4837-9A77-64BCEE2F7E34}"; //published
					}
					item.Editing.EndEdit();
				}
			}
		}

		/// <summary>
		/// Runs validators to determine if item is valid
		/// </summary>
		/// <param name="item">Item to validate</param>
		/// <returns>true if item is valid, false otherwise</returns>
		protected bool ItemPassesValidation(Item item)
		{
			ValidatorCollection validators = ValidatorManager.BuildValidators(ValidatorsMode.Workflow, item);
			ValidatorManager.Validate(validators, new ValidatorOptions(false));
			foreach (BaseValidator validator in validators)
			{
				if (validator.Result != ValidatorResult.Valid)
				{
					return false;
				}
			}

			return true;
		}
	}
}
