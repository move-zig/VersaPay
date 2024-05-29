namespace VersaPay;

public interface IPaymentReader
{
    public Payment ReadRow(int rowIndex);
}
