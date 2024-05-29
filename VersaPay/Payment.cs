namespace VersaPay;

public record Payment
{
    public string PaymentReference { get; set; }

    public string InvoiceNumber { get; set; }

    public string Date { get; set; }

    public string Amount { get; set; }

    public string PaymentAmount { get; set; }

    public string PaymentMethod { get; set; }

    public string ShortPayIndicator { get; set; }

    public string PaymentNote { get; set; }

    public string AutoDebitIndicator { get; set; }

    public string InvoiceBalance { get; set; }

    public string PaymentTimestamp { get; set; }

    public string InvoiceDivision { get; set; }

    public string InvoiceDivisionNumber { get; set; }

    public string PayToBankAccount { get; set; }

    public string CustomerId { get; set; }

    public string Status { get; set; }

    public string PaymentSource { get; set; }

    public string PaymentCode { get; set; }

    public string PaymentDescription { get; set; }
}
