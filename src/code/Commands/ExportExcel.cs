namespace Sitecore.Feature.Reports.Commands
{
  using Sitecore.Feature.Reports.Export;

  public class ExportExcel : ExportBaseCommand
  {
    protected override string GetFilePath()
    {
      var export = new XlsxExport(Current.Context.Report, Current.Context.ReportItem);
      var reportName = $"{Settings.Instance.ReportExportPrefix}{Current.Context.ReportItem.Name}";
      return export.SaveFile(reportName, "xlsx");
    }
  }
}