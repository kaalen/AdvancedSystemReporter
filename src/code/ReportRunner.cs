using Sitecore.Web.UI.Sheer;
using Sitecore.Feature.Reports.Interface;

namespace Sitecore.Feature.Reports
{
    public class ReportRunner
    {
        public void RunCommand(ClientPipelineArgs args)
        {
            //get parameters from the ui
            Sitecore.Context.ClientPage.SendMessage(this, "Feature.Reports.MainForm:updateparameters");

            Current.Context.Report = 
              Current.Context.ReportItem.TransformToReport(Current.Context.Report);         

            Sitecore.Shell.Applications.Dialogs.ProgressBoxes.ProgressBox.Execute(
                "Scanning...",
                "Scanning items",
                "",
                 Current.Context.Report.Run,
                "MainForm:runfinished",
                new object[] { });
        }
    }
}
