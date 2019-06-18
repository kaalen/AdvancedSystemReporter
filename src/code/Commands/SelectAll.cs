using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.Reports.Commands
{
  internal class SelectAll : Command
  {
    public override void Execute(CommandContext context)
    {
      Sitecore.Context.ClientPage.Start(this, "SendMessage");
    }

    public override CommandState QueryState(CommandContext context)
    {
      if (Current.Context.Report == null)
      {
        return CommandState.Disabled;
      }

      return base.QueryState(context);
    }

    /// <summary>
    /// Selects all items.
    /// </summary>
    /// <param name="args">The args.</param>
    public void SendMessage(ClientPipelineArgs args)
    {
      Message message = Message.Parse(this, "reports:selectall");
      Sitecore.Context.ClientPage.SendMessage(message);
    }
  }
}