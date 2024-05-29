namespace VersaPay.PaymentRepository;

using VersaPay;

public class DummyPaymentRepository : IPaymentRepository
{
    public void Save(Payment payment)
    {
        Console.WriteLine($"Saving {payment}");
    }
}
