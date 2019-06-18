using System.Linq;
using Sitecore.Tasks;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Feature.Reports.DomainObjects;

namespace Sitecore.Feature.Reports.Commands
{
  using System.Net.Mail;
  using CommandItem = Sitecore.Tasks.CommandItem;
  using Sitecore;

  public class ScheduledExecution
  {
    private const string LogPrefix = "Sitecore.Feature.Reports.Email -- ";

    public void EmailReports(Item[] itemarray, CommandItem commandItem, ScheduleItem scheduleItem)
    {
      var item = commandItem.InnerItem;
      if (item[Templates.ReportEmailTask.Fields.Active] != "1")
      {
        return;
      }

      Log("Starting task");
      MultilistField mf = item.Fields[Templates.ReportEmailTask.Fields.Reports];
      var format = item[Templates.ReportEmailTask.Fields.Format].ToLower();
      if (mf == null)
      {
        return;
      }
      var force = item[Templates.ReportEmailTask.Fields.SendEmpty] == "1";
      var filePaths = mf.GetItems().Select(i => this.RunReport(i, force, format));


      var mailMessage = new MailMessage
      {
        From = new MailAddress(item[Templates.ReportEmailTask.Fields.From]),
        Subject = item[Templates.ReportEmailTask.Fields.Subject],
      };
      var senders = item[Templates.ReportEmailTask.Fields.To].Split(',');
      foreach (var sender in senders)
      {
        mailMessage.To.Add(sender);
      }

      mailMessage.Body = Sitecore.Web.UI.WebControls.FieldRenderer.Render(item, "text");
      mailMessage.IsBodyHtml = true;

      foreach (var path in filePaths.Where(st => !string.IsNullOrEmpty(st)))
      {
        mailMessage.Attachments.Add(new Attachment(path));
      }
      Log("Attempting to send message");
      MainUtil.SendMail(mailMessage);
      Log("Task finished");
    }

    private void Log(string message)
    {
      Sitecore.Diagnostics.Log.Info(string.Concat(LogPrefix, message), this);
    }

    private string RunReport(Item item, bool force, string format)
    {
      Assert.IsNotNull(item, "item");
      var parameters = item["parameters"];
      var reportItem = ReportItem.CreateFromParameters(parameters);
      var prefix = reportItem.Name;
      var report = reportItem.TransformToReport(null);
      report.Run(null);
      Log(string.Concat("Run", reportItem.Name));

      if (report.ResultsCount() != 0 || force)
      {
        switch (format)
        {
          case "xlsx":
          case "excel":
            return new Export.XlsxExport(report, reportItem).SaveFile(prefix, "xlsx");
          case "html":
            return new Export.HtmlExport(report, reportItem).SaveFile(prefix, "html");
          case "csv":
            return new Export.CsvExport(report, reportItem).SaveFile(prefix, "csv");
          default:
            return new Export.HtmlExport(report, reportItem).SaveFile(prefix, "html");
        }
      }

      return null;
    }
  }
}