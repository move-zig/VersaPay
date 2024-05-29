namespace VersaPay;

public interface ICSVHandler
{
    public void ParseStoreAndCopyCSV(string csvFullName, string destinationFullName);
}
