namespace VersaPay.Spreadsheet;

public interface ISpreadsheet
{
    /// <summary>
    /// Gets the number of rows in the active worksheet.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Gets the number of columns in the active worksheet.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Sets the active worksheet.
    /// </summary>
    /// <param name="index">The zero-based index of the worksheet.</param>
    public void SetWorkSheet(int index);

    /// <summary>
    /// Reads a value from a cell.
    /// </summary>
    /// <param name="row">The zero-based row.</param>
    /// <param name="col">The zero-based column.</param>
    /// <returns>The the cell value, or null.</returns>
    public string GetString(int row, int col);

    /// <summary>
    /// Reads the type of value from a cell.
    /// </summary>
    /// <param name="row">The zero-based row.</param>
    /// <param name="col">The zero-based column.</param>
    /// <returns>The type of the cell value.</returns>
    public CellValueType GetCellValueType(int row, int col);

    /// <summary>
    /// Saves the document as a PDF.
    /// </summary>
    /// <param name="filename">The filename of the the PDF to create.</param>
    public void ExportPDF(string filename);

    /// <summary>
    /// Saves the document as a CSV.
    /// </summary>
    /// <param name="filename">The filename of the the CSV to create.</param>
    public void ExportCSV(string filename);
}