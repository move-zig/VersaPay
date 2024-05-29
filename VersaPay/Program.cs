namespace VersaPay;

using VersaPay.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VersaPay.PaymentRepository;
using System.IO.Abstractions;

public class Program
{
    private const string IncommingFolder = @"C:\Temp";
    private const string CopyFolder = @"C:\Temp\Copies";
    
    private readonly ISpreadsheetReader spreadsheetReader;
    private readonly IPaymentRepository paymentRepository;
    private readonly IFileSystem fileSystem;
    private readonly ILogger<Program> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    /// <param name="spreadsheetReader">An <see cref="ISpreadsheetReader"/>.</param>
    /// <param name="paymentRepository">An <see cref="IPaymentRepository"/>.</param>
    /// <param name="logger">An <see cref="ILogger"/>.</param>
    public Program(ISpreadsheetReader spreadsheetReader, IPaymentRepository paymentRepository, IFileSystem fileSystem, ILogger<Program> logger)
    {
        this.spreadsheetReader = spreadsheetReader;
        this.paymentRepository = paymentRepository;
        this.fileSystem = fileSystem;
        this.logger = logger;
    }

    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<ISpreadsheetReader, GemboxSpreadsheetReader>();
        builder.Services.AddSingleton<IPaymentRepository, DummyPaymentRepository>();
        builder.Services.AddSingleton<IFileSystem, FileSystem>();
        builder.Services.AddSingleton<Program>();

        using var host = builder.Build();

        var program = host.Services.GetRequiredService<Program>();

        await program.RunAsync();
    }

    public async Task RunAsync()
    {
        if (!this.fileSystem.Directory.Exists(CopyFolder))
        {
            this.fileSystem.Directory.CreateDirectory(CopyFolder);
        }
        
        var csvFiles = this.GetCSVFiles();

        foreach (var csvFile in csvFiles)
        {
            this.ParseStoreAndCopyCSV(csvFile);
        }
    }

    private void ParseStoreAndCopyCSV(IFileInfo fileInfo)
    {
        var spreadsheet = this.spreadsheetReader.ReadCSV(fileInfo.FullName);

        var paymentReader = new PaymentReader(spreadsheet);

        for (int row = 1; row < spreadsheet.RowCount; row++)
        {
            var payment = paymentReader.ReadRow(row);
            this.paymentRepository.Save(payment);
            Console.WriteLine(payment);
        }

        var destinationFilename = this.fileSystem.Path.Join(CopyFolder, fileInfo.Name);
        spreadsheet.ExportCSV(destinationFilename);
    }

    private IEnumerable<IFileInfo> GetCSVFiles()
    {
        return this.fileSystem.DirectoryInfo.New(IncommingFolder)
            .GetFiles()
            .Where(this.IsCSVFile);
    }

    private bool IsCSVFile(IFileInfo fileInfo)
    {
        return fileInfo.Extension.Equals(".csv", StringComparison.CurrentCultureIgnoreCase);
    }
}