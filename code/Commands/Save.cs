using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Data.Items;
using Sitecore.Web.UI.Sheer;
using Sitecore.Configuration;

namespace Sitecore.Feature.Reports.Commands
{
  public class Save : Command
  {
    public override void Execute(CommandContext context)
    {
      Sitecore.Context.ClientPage.Start(this, "Start");
    }

    public void Start(ClientPipelineArgs args)
    {
      if (args.IsPostBack)
      {
        Database database = Factory.GetDatabase(Settings.Instance.ConfigurationDatabase);
        Assert.IsNotNull(database, $"Configuration database {Settings.Instance.ConfigurationDatabase} is null.");

        Item report = database.GetItem(Current.Context.ReportItem.ID);

        Assert.IsNotNull(report, "can't find report item");

                Sitecore.Context.ClientPage.SendMessage(this, "Feature.Reports.MainForm:updateparameters");

        using (new SecurityModel.SecurityDisabler())
        {
          var savedReportTemplate = database.GetTemplate(Templates.SavedReport.ID);
          Item newItem = ItemUtil.AddFromTemplate(args.Result, savedReportTemplate.FullName, report);
          using (new EditContext(newItem))
          {
            newItem["parameters"] = Current.Context.ReportItem.SerializeParameters("^", "&");
            newItem[FieldIDs.Owner] = Sitecore.Context.User.Name;
          }
        }
      }
      else
      {
        Sitecore.Context.ClientPage.ClientResponse.Input("Enter the name of the saved report:",
          "Report Name", Configuration.Settings.ItemNameValidation, "'$Input' is not a valid name.",
          Configuration.Settings.MaxItemNameLength);
        args.WaitForPostBack(true);
      }
    }
  }
}