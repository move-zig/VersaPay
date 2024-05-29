namespace VersaPay;

using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using VersaPay.PaymentRepository;
using VersaPay.Spreadsheet;

/// <inheritdoc/>
public class CSVHandler : ICSVHandler
{
    private readonly ISpreadsheetReader spreadsheetReader;
    private readonly IPaymentRepository paymentRepository;
    private readonly IFileSystem fileSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="CSVHandler"/> class.
    /// </summary>
    /// <param name="spreadsheetReader">An <see cref="ISpreadsheetReader"/>.</param>
    /// <param name="paymentRepository">An <see cref="IPaymentRepository"/>.</param>
    /// <param name="logger">An <see cref="ILogger"/>.</param>
    public CSVHandler(ISpreadsheetReader spreadsheetReader, IPaymentRepository paymentRepository, IFileSystem fileSystem, ILogger<Program> logger)
    {
        this.spreadsheetReader = spreadsheetReader;
        this.paymentRepository = paymentRepository;
        this.fileSystem = fileSystem;
    }

    /// <inheritdoc/>
    public void ParseStoreAndCopyCSV(IFileInfo fileInfo, string copyFolder)
    {
        var spreadsheet = this.spreadsheetReader.ReadCSV(fileInfo.FullName);

        var paymentReader = new PaymentReader(spreadsheet);

        for (int row = 1; row < spreadsheet.RowCount; row++)
        {
            var payment = paymentReader.ReadRow(row);
            this.paymentRepository.Save(payment);
        }

        var destinationFilename = this.fileSystem.Path.Join(copyFolder, fileInfo.Name);
        spreadsheet.ExportCSV(destinationFilename);
    }
}
