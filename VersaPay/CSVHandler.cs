namespace VersaPay;

using VersaPay.PaymentRepository;
using VersaPay.Spreadsheet;

/// <inheritdoc/>
public class CSVHandler : ICSVHandler
{
    private readonly ISpreadsheetReader spreadsheetReader;
    private readonly IPaymentRepository paymentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CSVHandler"/> class.
    /// </summary>
    /// <param name="spreadsheetReader">An <see cref="ISpreadsheetReader"/>.</param>
    /// <param name="paymentRepository">An <see cref="IPaymentRepository"/>.</param>
    public CSVHandler(ISpreadsheetReader spreadsheetReader, IPaymentRepository paymentRepository)
    {
        this.spreadsheetReader = spreadsheetReader;
        this.paymentRepository = paymentRepository;
    }

    /// <inheritdoc/>
    public void ParseStoreAndCopyCSV(string csvFullName, string destinationFullName)
    {
        var spreadsheet = this.spreadsheetReader.ReadCSV(csvFullName);

        var paymentReader = new PaymentReader(spreadsheet);

        for (int row = 1; row < spreadsheet.RowCount; row++)
        {
            var payment = paymentReader.ReadRow(row);
            this.paymentRepository.Save(payment);
        }

        spreadsheet.ExportCSV(destinationFullName);
    }
}
