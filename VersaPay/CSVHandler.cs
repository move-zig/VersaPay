namespace VersaPay;

using VersaPay.PaymentRepository;
using VersaPay.Spreadsheet;

/// <inheritdoc/>
public class CSVHandler : ICSVHandler
{
    private readonly ISpreadsheetReader spreadsheetReader;
    private readonly IPaymentReaderFactory paymentReaderFactory;
    private readonly IPaymentRepository paymentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CSVHandler"/> class.
    /// </summary>
    /// <param name="spreadsheetReader">An <see cref="ISpreadsheetReader"/>.</param>
    /// <param name="paymentReaderFactory">An <see cref="IPaymentReaderFactory"/>.</param>
    /// <param name="paymentRepository">An <see cref="IPaymentRepository"/>.</param>
    public CSVHandler(ISpreadsheetReader spreadsheetReader, IPaymentReaderFactory paymentReaderFactory, IPaymentRepository paymentRepository)
    {
        this.spreadsheetReader = spreadsheetReader;
        this.paymentReaderFactory = paymentReaderFactory;
        this.paymentRepository = paymentRepository;
    }

    /// <inheritdoc/>
    public void ParseAndStore(string csvFullName)
    {
        var spreadsheet = this.spreadsheetReader.ReadCSV(csvFullName);

        var paymentReader = this.paymentReaderFactory.Create(spreadsheet);

        for (int row = 1; row < spreadsheet.RowCount; row++)
        {
            var payment = paymentReader.ReadRow(row);
            this.paymentRepository.Save(payment);
        }
    }
}
