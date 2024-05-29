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

    private readonly ICSVHandler csvHandler;
    private readonly IFileSystem fileSystem;
    private readonly ILogger<Program> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    /// <param name="csvHandler">An <see cref="ICSVHandler"/>.</param>
    /// <param name="fileSystem">An <see cref="IFileSystem"/>.</param>
    /// <param name="logger">An <see cref="ILogger"/>.</param>
    public Program(ICSVHandler csvHandler, IFileSystem fileSystem, ILogger<Program> logger)
    {
        this.csvHandler = csvHandler;
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
            this.csvHandler.ParseStoreAndCopyCSV(csvFile, CopyFolder);
        }
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