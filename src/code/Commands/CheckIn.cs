using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using System.Collections.Specialized;
using Sitecore.Web.UI.Sheer;
using Sitecore.SecurityModel;
using Sitecore.Data.Fields;

namespace Sitecore.Feature.Reports.Commands
{
	class CheckIn : Sitecore.Shell.Framework.Commands.CheckIn
	{
		public override CommandState QueryState(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			if (context.Items.Length != 1)
			{
				return CommandState.Hidden;
			}
			Item item = context.Items[0];
			if (!item.Access.CanWrite())
			{
				return CommandState.Disabled;
			}
			if (item.Appearance.ReadOnly)
			{
				return CommandState.Disabled;
			}
			if (!item.Locking.IsLocked())
			{
				return CommandState.Hidden;
			}
			return CommandState.Enabled;
		}

		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			if (context.Items.Length == 1)
			{
				Item item = context.Items[0];
				NameValueCollection parameters = new NameValueCollection();
				parameters["id"] = item.ID.ToString();
				parameters["language"] = item.Language.ToString();
				parameters["version"] = item.Version.ToString();
				Sitecore.Context.ClientPage.Start(this, "Run", parameters);
			}
		}

		protected new void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			string itemPath = args.Parameters["id"];
			string name = args.Parameters["language"];
			string version = args.Parameters["version"];
			Item item = Sitecore.Client.GetItemNotNull(itemPath, Sitecore.Globalization.Language.Parse(name), Sitecore.Data.Version.Parse(version));

			Log.Audit(this, "Check in: {0}", AuditFormatter.FormatItem(item));
			using (new SecurityDisabler())
			{
				item.Editing.BeginEdit();
				LockField lockField = item.Fields[FieldIDs.Lock];
				lockField.ReleaseLock();
				item.Editing.EndEdit();
			}
			Sitecore.Context.ClientPage.SendMessage(this, "item:checkedin");
			Sitecore.Context.ClientPage.SendMessage(this, "reports:refresh");
		}
	}
}
