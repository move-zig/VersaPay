namespace VersaPayTest;

using AutoBogus;
using Bogus;
using Moq;
using VersaPay;
using VersaPay.PaymentRepository;
using VersaPay.Spreadsheet;

public class CSVHandlerTest
{
    private readonly Mock<ISpreadsheetReader> spreadsheetReader = new ();
    private readonly Mock<IPaymentReaderFactory> paymentReaderFactory = new ();
    private readonly Mock<IPaymentRepository> paymentRepository = new ();

    private readonly Mock<ISpreadsheet> spreadsheet = new ();
    private readonly Mock<IPaymentReader> paymentReader = new ();

    private readonly CSVHandler csvHandler;

    private readonly Faker faker = new ();

    public CSVHandlerTest()
    {
        this.spreadsheetReader.Setup(x => x.ReadCSV(It.IsAny<string>())).Returns(this.spreadsheet.Object);
        this.paymentReaderFactory.Setup(x => x.Create(It.IsAny<ISpreadsheet>())).Returns(this.paymentReader.Object);

        this.csvHandler = new CSVHandler(this.spreadsheetReader.Object, this.paymentReaderFactory.Object, this.paymentRepository.Object);
    }

    [Fact]
    public void ParseAndStore_RunsOnEachRow()
    {
        var csvFullName = Path.Join(@"C:\Temp\Foo", this.faker.System.FileName("csv"));
        var rowCount = this.faker.Random.Int(8, 300);

        var payments = new List<Payment>();
        for (int i = 1; i < rowCount; i++)
        {
            var payment = AutoFaker.Generate<Payment>();
            payments.Add(payment);
            this.paymentReader.Setup(x => x.ReadRow(i)).Returns(payment);
        }

        this.spreadsheet.Setup(x => x.RowCount).Returns(rowCount);

        this.csvHandler.ParseAndStore(csvFullName);

        // correct CSV is opened
        this.spreadsheetReader.Verify(x => x.ReadCSV(csvFullName), Times.Once);

        // each payment is saved
        for (int i = 0; i < rowCount - 1; i++)
        {
            this.paymentRepository.Verify(x => x.Save(payments[i]), Times.Once);
        }

        // the correct number of payments were saved
        this.paymentRepository.Verify(x => x.Save(It.IsAny<Payment>()), Times.Exactly(rowCount - 1));
    }
}
