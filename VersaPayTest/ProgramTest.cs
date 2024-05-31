namespace VersaPayTest;

using Microsoft.Extensions.Logging;
using Moq;
using System.IO.Abstractions.TestingHelpers;
using VersaPay;

public class ProgramTest
{
    private const string IncommingDirectory = @"C:\Temp";
    private const string DestinationDirectory = @"C:\Backup";

    private readonly Mock<ICSVHandler> csvHandler = new ();
    private readonly Mock<ILogger<Program>> logger = new ();
    private readonly MockOptions<ProgramOptions> options;

    public ProgramTest()
    {
        this.options = new MockOptions<ProgramOptions>
        {
            Value = new ProgramOptions
            {
                DestinationDirectory = DestinationDirectory,
                IncommingDirectory = IncommingDirectory,
            },
        };

    }

    [Fact]
    public async Task RunAsync_CreatesDirectory()
    {
        var fileSystem = new MockFileSystem();

        var program = new Program(this.options, this.csvHandler.Object, fileSystem, this.logger.Object);

        await program.RunAsync();

        Assert.True(fileSystem.Directory.Exists(DestinationDirectory));
    }

    [Fact]
    public async Task RunAsync_CallsParseStoreAndCopyForEachCSVFile()
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { @"C:\Temp\foo.csv", new MockFileData("Hello!") },
            { @"C:\Temp\bar.csv", new MockFileData("Hello!") },
            { @"C:\Temp\not_a_csv.txt", new MockFileData("Hello!") }, // not a CSV file
            { @"C:\Temp\baz.csv", new MockFileData("Hello!") },
            { @"C:\OtherFolder\qux.csv", new MockFileData("Hello!") }, // wrong folder
        });

        var program = new Program(this.options, this.csvHandler.Object, fileSystem, this.logger.Object);
        await program.RunAsync();

        this.csvHandler.Verify(x => x.ParseAndStore(@"C:\Temp\foo.csv"), Times.Once());
        this.csvHandler.Verify(x => x.ParseAndStore(@"C:\Temp\bar.csv"), Times.Once());
        this.csvHandler.Verify(x => x.ParseAndStore(@"C:\Temp\baz.csv"), Times.Once());

        this.csvHandler.Verify(x => x.ParseAndStore(It.IsAny<string>()), Times.Exactly(3));

        Assert.False(fileSystem.File.Exists(@"C:\Temp\foo.csv"));
        Assert.False(fileSystem.File.Exists(@"C:\Temp\bar.csv"));
        Assert.False(fileSystem.File.Exists(@"C:\Temp\baz.csv"));

        Assert.True(fileSystem.File.Exists(Path.Join(DestinationDirectory, "foo.csv")));
        Assert.True(fileSystem.File.Exists(Path.Join(DestinationDirectory, "bar.csv")));
        Assert.True(fileSystem.File.Exists(Path.Join(DestinationDirectory, "baz.csv")));
    }
}