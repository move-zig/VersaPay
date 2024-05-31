namespace VersaPay;

using VersaPay.Spreadsheet;

public interface IPaymentReaderFactory
{
    public IPaymentReader Create(ISpreadsheet spreadsheet);
}
