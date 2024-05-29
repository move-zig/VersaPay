namespace VersaPay;

using System.IO.Abstractions;

public interface ICSVHandler
{
    public void ParseStoreAndCopyCSV(IFileInfo fileInfo, string copyFolder);
}
