namespace VersaPay.PaymentRepository;

using VersaPay;

public interface IPaymentRepository
{
    public void Save(Payment payment);
}
