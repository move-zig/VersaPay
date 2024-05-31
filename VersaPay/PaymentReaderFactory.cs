namespace VersaPay;

using VersaPay.Spreadsheet;

/// <inheritdoc />
public class PaymentReaderFactory : IPaymentReaderFactory
{
    /// <inheritdoc />
    public IPaymentReader Create(ISpreadsheet spreadsheet)
    {
        return new PaymentReader(spreadsheet);
    }
}
