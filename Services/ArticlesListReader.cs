using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace material_manager.Services;

public class ArticlesListReader : IArticlesListReader
{
    public List<ArticleLine> ReadExcelFile(byte[] bytes)
    {
        var result = new List<ArticleLine>();

        using (var ms = new MemoryStream(bytes))
        using (var spreadsheetDocument = SpreadsheetDocument.Open(ms, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            if (workbookPart == null)
            {
                throw new Exception("Did not find workbook part");
            }
            var worksheetPart = workbookPart.WorksheetParts?.FirstOrDefault();
            if (worksheetPart == null)
            {
                throw new Exception("Did not find worksheet part");
            }
            if (worksheetPart.Worksheet == null)
            {
                throw new Exception("Expected worksheet but did not get it.");
            }
            var sheetData = worksheetPart.Worksheet!.Elements<SheetData>().FirstOrDefault();
            if (sheetData == null)
            {
                throw new Exception("Did not find sheet data");
            }
            var sharedStringTable = workbookPart.SharedStringTablePart;
            if (sharedStringTable == null)
            {
                throw new Exception("Expected shared string table part");
            }

            var rows = sheetData.Elements<Row>();
            // skip header row
            rows = rows.Skip(1);
            foreach (Row r in rows)
            {
                var articleLine = ImportRow(sharedStringTable.SharedStringTable!, r);
                if (articleLine != null)
                {
                    result.Add(articleLine);
                }
            }
        }
        return result;
    }

        private ArticleLine? ImportRow(SharedStringTable sharedStringTable, Row r)
    {
        var cells = r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().ToList();
        if (cells == null)
        {
            return null;
        }

        DocumentFormat.OpenXml.Spreadsheet.Cell? findCellByColumn(string columnName)
        {
            return cells.FirstOrDefault(c => c.CellReference != null &&
                c.CellReference.Value != null &&
                c.CellReference.Value.StartsWith(columnName));
        }

        var articleReferenceCell = findCellByColumn("A");
        var supplierReferenceCell = findCellByColumn("B");
        var mnemonicCell = findCellByColumn("F");
        var labelCell = findCellByColumn("G");
        var eanCell = findCellByColumn("H");
        var serialNumberCell = findCellByColumn("I");
        var amountCell = findCellByColumn("J");
        var nextControlCell = findCellByColumn("K");
        if (mnemonicCell == null || labelCell == null || eanCell == null || articleReferenceCell == null)
        {
            return null;
        }

        string? getStringContent(DocumentFormat.OpenXml.Spreadsheet.Cell c)
        {
            if (c.CellValue == null ||
                c.DataType == null)
            {
                return null;
            }
            if (c.DataType.Value != CellValues.SharedString)
            {
                return c.CellValue.InnerText;
            }
            return sharedStringTable.ElementAt(int.Parse(c.CellValue.InnerText)).InnerText;
        }

        int? getIntContent(DocumentFormat.OpenXml.Spreadsheet.Cell c)
        {
            if (c.CellValue == null ||
                c.DataType == null)
            {
                return null;
            }
            return int.Parse(c.CellValue.InnerText);
        }

        DateOnly? getDateContent(DocumentFormat.OpenXml.Spreadsheet.Cell c)
        {
            if (c.CellValue == null ||
                c.DataType == null)
            {
                return null;
            }
            return DateOnly.Parse(c.CellValue.InnerText, new CultureInfo("fr-CH"));
        }

        string mnemonic = getStringContent(mnemonicCell) ?? String.Empty;
        string label = getStringContent(labelCell) ?? String.Empty;
        string ean = getStringContent(eanCell) ?? String.Empty;
        string articleReference = getStringContent(articleReferenceCell) ?? String.Empty;
        string supplierReference = (supplierReferenceCell == null ?
            String.Empty :
            getStringContent(supplierReferenceCell)) ?? String.Empty;
        string serialNumber = (serialNumberCell == null ?
            String.Empty : getStringContent(serialNumberCell)) ?? String.Empty;
        int amount = amountCell == null ? 0 : (getIntContent(amountCell) ?? 0);
        DateOnly? nextControl = (nextControlCell == null ? null :
            getDateContent(nextControlCell));

        if (String.IsNullOrWhiteSpace(ean))
        {
            return null;
        }

        return new ArticleLine(articleReference, supplierReference, mnemonic,
        label, ean, serialNumber, amount, nextControl);
    }
}
