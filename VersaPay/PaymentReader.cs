namespace VersaPay;

using VersaPay.Spreadsheet;

/// <inheritdoc/>
public class PaymentReader : IPaymentReader
{
    /// <summary>The maximum number of columns to search.</summary>
    private const int MaxColumns = 30;

    private readonly string[] headings;
    private readonly ISpreadsheet spreadsheet;

    public PaymentReader(ISpreadsheet spreadsheet)
    {
        this.spreadsheet = spreadsheet;

        this.headings = new string[MaxColumns];

        for (int col = 0; col < MaxColumns; col++)
        {
            if (this.spreadsheet.GetCellValueType(0, col) != CellValueType.String)
            {
                continue;
            }

            this.headings[col] = this.spreadsheet.GetString(0, col);
        }
    }

    /// <inheritdoc/>
    public Payment ReadRow(int row)
    {
        var payment = new Payment();

        for (var col = 0; col < MaxColumns; col++)
        {
            if (this.spreadsheet.GetCellValueType(0, col) != CellValueType.String)
            {
                continue;
            }

            var trimmedValue = this.spreadsheet.GetString(row, col).Trim();

            switch (this.headings[col])
            {
                case "payment_reference":
                    payment.PaymentReference = trimmedValue;
                    break;
                case "invoice_number":
                    payment.InvoiceNumber = trimmedValue;
                    break;
                case "date":
                    payment.Date = trimmedValue;
                    break;
                case "amount":
                    payment.Amount = trimmedValue;
                    break;
                case "payment_amount":
                    payment.PaymentAmount = trimmedValue;
                    break;
                case "payment_method":
                    payment.PaymentMethod = trimmedValue;
                    break;
                case "short_pay_indicator":
                    payment.ShortPayIndicator = trimmedValue;
                    break;
                case "payment_note":
                    payment.PaymentNote = trimmedValue.Length > 1000 ? trimmedValue[..999] : trimmedValue;
                    break;
                case "auto_debit_indicator":
                    payment.AutoDebitIndicator = trimmedValue;
                    break;
                case "invoice_balance":
                    payment.InvoiceBalance = trimmedValue;
                    break;
                case "payment_timestamp":
                    payment.PaymentTimestamp = trimmedValue;
                    break;
                case "invoice_division":
                    payment.InvoiceDivision = trimmedValue;
                    break;
                case "invoice_division_number":
                    payment.InvoiceDivisionNumber = trimmedValue;
                    break;
                case "pay_to_bank_account":
                    payment.PayToBankAccount = trimmedValue;
                    break;
                case "customer_identifier":
                    payment.CustomerId = trimmedValue;
                    break;
                case "status":
                    payment.Status = trimmedValue;
                    break;
                case "payment_source":
                    payment.PaymentSource = trimmedValue;
                    break;
                case "payment_code":
                    payment.PaymentCode = trimmedValue;
                    break;
                case "payment_description":
                    payment.PaymentDescription = trimmedValue;
                    break;
            }
        }

        return payment;
    }
}
