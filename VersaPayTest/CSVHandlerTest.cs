namespace VersaPayTest;

using Bogus;
using Moq;
using VersaPay;
using VersaPay.PaymentRepository;
using VersaPay.Spreadsheet;

public class CSVHandlerTest
{
    private readonly Mock<IPaymentRepository> paymentRepository = new ();
    private readonly Mock<ISpreadsheetReader> spreadsheetReader = new ();

    private readonly Mock<PaymentReader> paymentReader = new ();

    private readonly Mock<ISpreadsheet> spreadsheet = new ();

    private readonly CSVHandler csvHandler;

    private readonly Faker faker = new ();

    public CSVHandlerTest()
    {
        this.spreadsheetReader.Setup(x => x.ReadCSV(It.IsAny<string>())).Returns(this.spreadsheet.Object);
        this.csvHandler = new CSVHandler(this.spreadsheetReader.Object, this.paymentRepository.Object);
    }

    [Fact]
    public void ParseAndStore_RunsOnEachRow()
    {
        var csvFullName = Path.Join(@"C:\Temp\Foo", this.faker.System.FileName("csv"));
        var rowCount = this.faker.Random.Int(8, 300);

        this.spreadsheet.Setup(x => x.RowCount).Returns(rowCount);

        this.csvHandler.ParseAndStore(csvFullName);

        this.spreadsheetReader.Verify(x => x.ReadCSV(csvFullName), Times.Once());

        this.paymentRepository.Verify(x => x.Save(It.IsAny<Payment>()), Times.Exactly(rowCount - 1));
    }
}
