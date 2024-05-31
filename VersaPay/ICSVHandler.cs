namespace VersaPay;

public interface ICSVHandler
{
    public void ParseAndStore(string csvFullName);
}
