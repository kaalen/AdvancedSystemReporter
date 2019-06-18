using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.Feature.Reports.Export
{
  using Sitecore.Feature.Reports.DomainObjects;
  using Sitecore.Feature.Reports.Interface;

  internal class XMLExport : IExport
  {
    protected Report report;
    protected ReportItem reportItem;

    public XMLExport(Report report, ReportItem reportItem)
    {
      this.report = report;
      this.reportItem = reportItem;
    }

    public string SaveFile(string prefix, string extension)
    {
      System.IO.StringWriter oStringWriter = new System.IO.StringWriter();

      string tempPath =
        Sitecore.IO.FileUtil.GetWorkFilename(Sitecore.Configuration.Settings.TempFolderPath, prefix, extension);


      writeValues(oStringWriter);

      System.IO.File.WriteAllText(tempPath, oStringWriter.ToString());

      return tempPath;
    }

    private void writeValues(System.IO.StringWriter oStringWriter)
    {
      System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
      settings.Encoding = Encoding.UTF8;
      settings.ConformanceLevel = System.Xml.ConformanceLevel.Auto;

      System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(oStringWriter, settings);

      writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
      writer.WriteStartElement("report");
      writer.WriteAttributeString("date", DateTime.Now.ToString("yyyyMMdd_HHmm"));
      foreach (var row in this.report.GetResultElements())
      {
        writer.WriteStartElement("result");
        foreach (var col in row.GetColumnNames())
        {
          writer.WriteStartElement("column");
          writer.WriteAttributeString("name", col);
          writer.WriteString(row.GetColumnValue(col));
          writer.WriteEndElement();
        }

        writer.WriteEndElement();
      }
      writer.WriteEndElement();

      writer.Flush();
    }
  }
}