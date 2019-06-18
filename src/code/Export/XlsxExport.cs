using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sitecore.Feature.Reports.DomainObjects;
using Sitecore;
using Sitecore.Security.Accounts;
using Sitecore.IO;
using Sitecore.Feature.Reports.Interface;

namespace Sitecore.Feature.Reports.Export
{
  using System.IO;
  using DocumentFormat.OpenXml;
  using DocumentFormat.OpenXml.Packaging;
  using DocumentFormat.OpenXml.Spreadsheet;

  public class XlsxExport : IExport
  {
    protected Report report;
    protected ReportItem reportItem;

    public XlsxExport(Report report, ReportItem reportItem)
    {
      this.report = report;
      this.reportItem = reportItem;
    }

    public string SaveFile(string prefix, string extension)
    {
      var results = report.GetResultElements();
      var headers = GetHeaders(results);

      foreach (var dElement in results)
      {
        foreach (var header in dElement.GetColumnNames())
        {
          headers.Add(header);
        }
      }

      var tempPath = FileUtil.GetWorkFilename(Sitecore.Configuration.Settings.TempFolderPath, prefix, extension);
      using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(tempPath, SpreadsheetDocumentType.Workbook))
      {
        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet(new SheetData());

        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Report" };

        sheets.Append(sheet);

        workbookPart.Workbook.Save();

        SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        //construct report header
        ConstructHeaderRow(sheetData, headers);

        //construct report rows
        ConstructDataRows(sheetData, headers, results);

        worksheetPart.Worksheet.Save();
      }

      return tempPath;
    }

    private HashSet<string> GetHeaders(IEnumerable<DisplayElement> results)
    {
      var headers = new HashSet<string>();

      foreach (var dElement in results)
      {
        foreach (var header in dElement.GetColumnNames())
        {
          headers.Add(header);
        }
      }

      return headers;
    }

    private void ConstructHeaderRow(SheetData sheetData, HashSet<string> headers)
    {
      // Constructing header
      Row row = new Row();

      foreach (var header in headers)
      {
        var headerCell = ConstructCell(header, CellValues.String);
        row.Append(headerCell);
      }

      // Insert the header row to the Sheet Data
      sheetData.AppendChild(row);
    }

    private void ConstructDataRows(SheetData sheetData, HashSet<string> headers, IEnumerable<DisplayElement> results)
    {
      // Inserting each employee
      foreach (var result in results)
      {
        var row = new Row();

        foreach (var header in headers)
        {
          var cellValue = result.GetColumnValue(header);
          row.Append(ConstructCell(cellValue, CellValues.String));
        }

        sheetData.AppendChild(row);
      }
    }

    private Cell ConstructCell(string value, CellValues dataType)
    {
      return new Cell()
      {
        CellValue = new CellValue(value),
        DataType = new EnumValue<CellValues>(dataType)
      };
    }
  }
}