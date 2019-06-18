using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports.Commands
{
  using Sitecore.Feature.Reports.Export;

  public class ExportHtml : ExportBaseCommand
  {
    protected override string GetFilePath()
    {
      var export = new HtmlExport(Current.Context.Report, Current.Context.ReportItem);
      var reportName = $"{Settings.Instance.ReportExportPrefix}{Current.Context.ReportItem.Name}";
      return export.SaveFile(reportName, "html");
    }
  }
}