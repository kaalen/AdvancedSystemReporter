using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.Reports.Commands
{
  using Sitecore.Feature.Reports.Export;

  internal class ExportXML : ExportBaseCommand
  {
    protected override string GetFilePath()
    {
      var export = new XMLExport(Current.Context.Report, Current.Context.ReportItem);
      var reportName = $"{Settings.Instance.ReportExportPrefix}{Current.Context.ReportItem.Name}";
      return export.SaveFile(reportName, "xml");
    }
  }
}