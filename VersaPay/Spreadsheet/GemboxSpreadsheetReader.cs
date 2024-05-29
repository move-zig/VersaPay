namespace VersaPay.Spreadsheet;

using GemBox.Spreadsheet;
using System.Text;

/// <inheritdoc/>
public class GemboxSpreadsheetReader : ISpreadsheetReader
{
    static GemboxSpreadsheetReader()
    {
        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
    }

    /// <inheritdoc/>
    public ISpreadsheet ReadExcel(string filename)
    {
        var workbook = ExcelFile.Load(filename);

        return new GemboxSpreadsheet(workbook);
    }

    /// <inheritdoc/>
    public ISpreadsheet ReadCSV(string filename)
    {
        var loadOptions = new CsvLoadOptions(CsvType.CommaDelimited)
        {
            ParseNumbers = false,  // Treat all numbers as strings because we need to keep any leading zeroes
            ParseDates = false,  // Treat all dates as strings so their format isn't changed
        };

        var workbook = ExcelFile.Load(filename, loadOptions);

        return new GemboxSpreadsheet(workbook);
    }

    /// <inheritdoc/>
    internal class GemboxSpreadsheet(ExcelFile excelFile) : ISpreadsheet
    {
        private readonly ExcelFile excelFile = excelFile;
        private ExcelWorksheet worksheet = excelFile.Worksheets[0];

        /// <inheritdoc/>
        public int RowCount => this.worksheet.Rows.Count;

        /// <inheritdoc/>
        public int ColumnCount => this.worksheet.Columns.Count;

        /// <inheritdoc/>
        public void SetWorkSheet(int index)
        {
            this.worksheet = this.excelFile.Worksheets[index];
        }

        /// <inheritdoc/>
        public string GetString(int row, int col)
        {
            return this.worksheet.Cells[row, col].StringValue;
        }

        /// <inheritdoc/>
        public CellValueType GetCellValueType(int row, int col)
        {
            return this.worksheet.Cells[row, col].ValueType switch
            {
                GemBox.Spreadsheet.CellValueType.Bool => CellValueType.Bool,
                GemBox.Spreadsheet.CellValueType.DateTime => CellValueType.DateTime,
                GemBox.Spreadsheet.CellValueType.Double => CellValueType.Double,
                GemBox.Spreadsheet.CellValueType.Error => CellValueType.Error,
                GemBox.Spreadsheet.CellValueType.Int => CellValueType.Int,
                GemBox.Spreadsheet.CellValueType.Null => CellValueType.Null,
                GemBox.Spreadsheet.CellValueType.Object => CellValueType.Object,
                GemBox.Spreadsheet.CellValueType.String => CellValueType.String,
                _ => throw new Exception("Unexpected cell value type")
            };
        }

        /// <inheritdoc/>
        public void ExportPDF(string filename)
        {
            this.excelFile.Save(filename, new PdfSaveOptions());
        }

        /// <inheritdoc/>
        public void ExportCSV(string filename)
        {
            var saveOptions = new CsvSaveOptions(CsvType.CommaDelimited)
            {
                Encoding = new UTF8Encoding(false),
            };

            this.excelFile.Save(filename, saveOptions);
        }
    }
}
