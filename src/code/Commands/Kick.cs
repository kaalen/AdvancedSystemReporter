using Sitecore.Collections;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.Authentication;

namespace Sitecore.Feature.Reports.Sessions
{
	/// <summary>
	/// Implements a command which kicks a user or terminates his/her session.
	/// </summary>
	public class Kick : Command
	{

		public override void Execute(CommandContext context)
		{
			StringList sl = context.CustomData as StringList;
			if (sl != null)
			{
				foreach (var sessionId in sl)
				{
					DomainAccessGuard.Kick(sessionId);
				}
				Sitecore.Context.ClientPage.SendMessage(this, "reports:refresh");
			}
		}
	}
}
