using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Feature.Reports.Interface;

namespace Sitecore.Feature.Reports.Export
{
  using Sitecore.Feature.Reports.DomainObjects;

  public class CsvExport : IExport
  {
    protected Report report;
    protected ReportItem reportItem;

    public CsvExport(Report report, ReportItem reportItem)
    {
      this.report = report;
      this.reportItem = reportItem;
    }

    public char Separator
    {
      get
      {
        return ',';
      }
    }

    public string SaveFile(string prefix, string extension)
    {
      System.IO.StringWriter oStringWriter = new System.IO.StringWriter();

      string tempPath =
        Sitecore.IO.FileUtil.GetWorkFilename(Sitecore.Configuration.Settings.TempFolderPath, prefix, extension);

      HashSet<string> headers = new HashSet<string>();
      IEnumerable<DisplayElement> results = this.report.GetResultElements();
      foreach (var dElement in results)
      {
        foreach (var header in dElement.GetColumnNames())
        {
          headers.Add(header);
        }
      }
      this.WriteHeader(oStringWriter, headers);
      this.WriteValues(oStringWriter, headers, results);

      System.IO.File.WriteAllText(tempPath, oStringWriter.ToString());

      return tempPath;
    }

    private void WriteHeader(System.IO.StringWriter writer, IEnumerable<string> headers)
    {
      StringBuilder sb = new StringBuilder();
      foreach (var header in headers)
      {
        sb.AppendFormat("{0}{1}", Separator, header);
      }
      writer.WriteLine(sb.ToString().Substring(1));
    }

    private void WriteValues(System.IO.StringWriter writer, IEnumerable<string> headers, IEnumerable<DisplayElement> results)
    {
      foreach (var row in results)
      {
        IEnumerator<string> enumerator = headers.GetEnumerator();
        if (enumerator.MoveNext())
        {
          writer.Write(row.GetColumnValue(enumerator.Current));
          while (enumerator.MoveNext())
          {
            writer.Write(Separator);
            writer.Write(GetColumnValue(row, enumerator));
          }
          writer.WriteLine();
        }
      }
    }

    private static string GetColumnValue(DisplayElement row, IEnumerator<string> enumerator)
    {
      var value = row.GetColumnValue(enumerator.Current);
      return value.Replace('\n', '|').Replace('\r', '|');
    }
  }
}