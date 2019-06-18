using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using Sitecore.Feature.Reports.DomainObjects;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace Sitecore.Feature.Reports.Commands
{
  internal class Open : Command
  {
    public override void Execute(CommandContext context)
    {
      Sitecore.Context.ClientPage.Start(this, "Start");
    }

    public void Start(ClientPipelineArgs args)
    {
      if (!args.IsPostBack)
      {
        Util.ShowItemBrowser(
          "Select the report",
          "Select the report",
          "Database/32x32/view_h.png",
          "Select",
          Current.Context.Settings.ReportsFolder,
          Current.Context.ReportItem == null ? Current.Context.Settings.ReportsFolder : Current.Context.ReportItem.Path,
          Current.Context.Settings.ConfigurationDatabase);
        args.WaitForPostBack();
      }
      else
      {
        if (!ID.IsID(args.Result))
        {
          return;
        }
        Database database = Configuration.Factory.GetDatabase(Current.Context.Settings.ConfigurationDatabase);
        Diagnostics.Assert.IsNotNull(database, "no configuration databsae");

        Item item = database.GetItem(args.Result);

        Diagnostics.Assert.IsNotNull(item, "Report item cannot be loaded");

        switch (item.Template.Key)
        {
          case "report":


            ReportItem rItem = new ReportItem(item);
            if (rItem != null)
            {
              Current.Context.ReportItem = rItem;
              Current.Context.Report = null;
              Sitecore.Context.ClientPage.SendMessage(this, "Feature.Reports.MainForm:update");
            }
            break;

          case "saved report":

            Message m = Message.Parse(this, "Feature.Reports.MainForm:openlink");
            System.Collections.Specialized.NameValueCollection nvc =
              StringUtil.ParseNameValueCollection(item["parameters"], '&', '=');

            m.Arguments.Add(nvc);
            Sitecore.Context.ClientPage.SendMessage(m);
            break;
        }
      }
    }
  }
}