namespace VersaPay.Spreadsheet;

public interface ISpreadsheetReader
{
    /// <summary>
    /// Opens an excel document and sets the first worksheet as active.
    /// </summary>
    /// <param name="filename">The filename of the document to open.</param>
    /// <returns>The excel document.</returns>
    public ISpreadsheet ReadExcel(string filename);

    /// <summary>
    /// Opens an CSV file.
    /// </summary>
    /// <param name="filename">The filename of the document to open.</param>
    /// <returns>The excel document.</returns>
    public ISpreadsheet ReadCSV(string filename);
}
