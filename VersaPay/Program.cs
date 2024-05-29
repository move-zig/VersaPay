namespace VersaPay;

using VersaPay.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VersaPay.PaymentRepository;
using System.IO.Abstractions;
using Microsoft.Extensions.Options;

public class Program
{
    private readonly ProgramOptions options;
    private readonly ICSVHandler csvHandler;
    private readonly IFileSystem fileSystem;
    private readonly ILogger<Program> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    /// <param name="options">The configuraion options.</param>
    /// <param name="csvHandler">An <see cref="ICSVHandler"/>.</param>
    /// <param name="fileSystem">An <see cref="IFileSystem"/>.</param>
    /// <param name="logger">An <see cref="ILogger"/>.</param>
    public Program(IOptions<ProgramOptions> options, ICSVHandler csvHandler, IFileSystem fileSystem, ILogger<Program> logger)
    {
        this.options = options.Value;
        this.csvHandler = csvHandler;
        this.fileSystem = fileSystem;
        this.logger = logger;
    }

    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<ICSVHandler, CSVHandler>();
        builder.Services.AddSingleton<ISpreadsheetReader, GemboxSpreadsheetReader>();
        builder.Services.AddSingleton<IPaymentRepository, DummyPaymentRepository>();
        builder.Services.AddSingleton<IFileSystem, FileSystem>();
        builder.Services.AddOptions<ProgramOptions>()
            .Bind(builder.Configuration.GetSection(ProgramOptions.ConfigurationSectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddSingleton<Program>();

        using var host = builder.Build();

        var program = host.Services.GetRequiredService<Program>();

        await program.RunAsync();
    }

    public async Task RunAsync()
    {
        if (!this.fileSystem.Directory.Exists(this.options.DestinationDirectory))
        {
            this.fileSystem.Directory.CreateDirectory(this.options.DestinationDirectory);
        }

        var csvFileInfos = this.GetCSVFiles();

        foreach (var csvFileInfo in csvFileInfos)
        {
            var destinationFullName = this.fileSystem.Path.Join(this.options.DestinationDirectory, csvFileInfo.Name);
            this.csvHandler.ParseStoreAndCopyCSV(csvFileInfo.FullName, destinationFullName);
        }
    }

    private IEnumerable<IFileInfo> GetCSVFiles()
    {
        return this.fileSystem.DirectoryInfo.New(this.options.IncommingDirectory)
            .GetFiles()
            .Where(this.IsCSVFile);
    }

    private bool IsCSVFile(IFileInfo fileInfo)
    {
        return fileInfo.Extension.Equals(".csv", StringComparison.CurrentCultureIgnoreCase);
    }
}