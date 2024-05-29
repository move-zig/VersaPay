namespace VersaPay;

using VersaPay.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VersaPay.PaymentRepository;

public class Program
{
    private readonly ISpreadsheetReader spreadsheetReader;
    private readonly IPaymentRepository paymentRepository;
    private readonly ILogger<Program> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    /// <param name="spreadsheetReader">An <see cref="ISpreadsheetReader"/>.</param>
    /// <param name="paymentRepository">An <see cref="IPaymentRepository"/>.</param>
    /// <param name="logger">An <see cref="ILogger"/>.</param>
    public Program(ISpreadsheetReader spreadsheetReader, IPaymentRepository paymentRepository, ILogger<Program> logger)
    {
        this.spreadsheetReader = spreadsheetReader;
        this.paymentRepository = paymentRepository;
        this.logger = logger;
    }

    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<ISpreadsheetReader, GemboxSpreadsheetReader>();
        builder.Services.AddSingleton<IPaymentRepository, DummyPaymentRepository>();
        builder.Services.AddSingleton<Program>();

        using var host = builder.Build();

        var program = host.Services.GetRequiredService<Program>();

        await program.RunAsync();
    }

    public async Task RunAsync()
    {
        var spreadsheet = this.spreadsheetReader.ReadCSV(@"C:\temp\foo.csv");

        var paymentReader = new PaymentReader(spreadsheet);

        for (int row = 1; row < spreadsheet.RowCount; row++)
        {
            var payment = paymentReader.ReadRow(row);
            this.paymentRepository.Save(payment);
            Console.WriteLine(payment);
        }

        spreadsheet.ExportCSV(@"C:\temp\foo copy.csv");
    }
}