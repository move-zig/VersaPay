namespace VersaPayTest;

using VersaPay.Spreadsheet;
using Moq;
using System;
using VersaPay;

public class PaymentReaderTest
{
    private readonly Mock<ISpreadsheet> spreadsheet = new ();

    public PaymentReaderTest()
    {
        this.spreadsheet.Setup(x => x.RowCount).Returns(3);

        this.spreadsheet.Setup(x => x.GetCellValueType(It.IsAny<int>(), It.IsAny<int>())).Returns(CellValueType.Null);
        this.spreadsheet.Setup(x => x.GetString(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Not a string!"));

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 18; col++)
            {
                this.spreadsheet.Setup(x => x.GetCellValueType(row, col)).Returns(CellValueType.String);
            }
        }

        this.spreadsheet.Setup(x => x.GetString(0, 0)).Returns("payment_reference");
        this.spreadsheet.Setup(x => x.GetString(0, 1)).Returns("invoice_number");
        this.spreadsheet.Setup(x => x.GetString(0, 2)).Returns("date");
        this.spreadsheet.Setup(x => x.GetString(0, 3)).Returns("amount");
        this.spreadsheet.Setup(x => x.GetString(0, 4)).Returns("payment_amount");
        this.spreadsheet.Setup(x => x.GetString(0, 5)).Returns("short_pay_indicator");
        this.spreadsheet.Setup(x => x.GetString(0, 6)).Returns("payment_note");
        this.spreadsheet.Setup(x => x.GetString(0, 7)).Returns("auto_debit_indicator");
        this.spreadsheet.Setup(x => x.GetString(0, 8)).Returns("invoice_balance");
        this.spreadsheet.Setup(x => x.GetString(0, 9)).Returns("payment_timestamp");
        this.spreadsheet.Setup(x => x.GetString(0, 10)).Returns("invoice_division");
        this.spreadsheet.Setup(x => x.GetString(0, 11)).Returns("invoice_division_number");
        this.spreadsheet.Setup(x => x.GetString(0, 12)).Returns("pay_to_bank_account");
        this.spreadsheet.Setup(x => x.GetString(0, 13)).Returns("customer_identifier");
        this.spreadsheet.Setup(x => x.GetString(0, 14)).Returns("status");
        this.spreadsheet.Setup(x => x.GetString(0, 15)).Returns("payment_source");
        this.spreadsheet.Setup(x => x.GetString(0, 16)).Returns("payment_code");
        this.spreadsheet.Setup(x => x.GetString(0, 17)).Returns("payment_description");

        this.spreadsheet.Setup(x => x.GetString(1, 0)).Returns("f");
        this.spreadsheet.Setup(x => x.GetString(1, 1)).Returns("g");
        this.spreadsheet.Setup(x => x.GetString(1, 2)).Returns("gb");
        this.spreadsheet.Setup(x => x.GetString(1, 3)).Returns("g");
        this.spreadsheet.Setup(x => x.GetString(1, 4)).Returns("e");
        this.spreadsheet.Setup(x => x.GetString(1, 5)).Returns("3");
        this.spreadsheet.Setup(x => x.GetString(1, 6)).Returns("g");
        this.spreadsheet.Setup(x => x.GetString(1, 7)).Returns("f");
        this.spreadsheet.Setup(x => x.GetString(1, 8)).Returns("fd");
        this.spreadsheet.Setup(x => x.GetString(1, 9)).Returns("d");
        this.spreadsheet.Setup(x => x.GetString(1, 10)).Returns("f");
        this.spreadsheet.Setup(x => x.GetString(1, 11)).Returns("f");
        this.spreadsheet.Setup(x => x.GetString(1, 12)).Returns("f3");
        this.spreadsheet.Setup(x => x.GetString(1, 13)).Returns("4f");
        this.spreadsheet.Setup(x => x.GetString(1, 14)).Returns("f");
        this.spreadsheet.Setup(x => x.GetString(1, 15)).Returns("g");
        this.spreadsheet.Setup(x => x.GetString(1, 16)).Returns("ds");
        this.spreadsheet.Setup(x => x.GetString(1, 17)).Returns("d");

        this.spreadsheet.Setup(x => x.GetString(2, 0)).Returns("lorem");
        this.spreadsheet.Setup(x => x.GetString(2, 1)).Returns("ipsum");
        this.spreadsheet.Setup(x => x.GetString(2, 2)).Returns("dolor");
        this.spreadsheet.Setup(x => x.GetString(2, 3)).Returns("sit");
        this.spreadsheet.Setup(x => x.GetString(2, 4)).Returns("amet");
        this.spreadsheet.Setup(x => x.GetString(2, 5)).Returns("consectetur");
        this.spreadsheet.Setup(x => x.GetString(2, 6)).Returns("adipiscing");
        this.spreadsheet.Setup(x => x.GetString(2, 7)).Returns("elit");
        this.spreadsheet.Setup(x => x.GetString(2, 8)).Returns("Quisque");
        this.spreadsheet.Setup(x => x.GetString(2, 9)).Returns("porttitor");
        this.spreadsheet.Setup(x => x.GetString(2, 10)).Returns("porttitor");
        this.spreadsheet.Setup(x => x.GetString(2, 11)).Returns("tortor");
        this.spreadsheet.Setup(x => x.GetString(2, 12)).Returns("non");
        this.spreadsheet.Setup(x => x.GetString(2, 13)).Returns("auctor");
        this.spreadsheet.Setup(x => x.GetString(2, 14)).Returns("Aliquam");
        this.spreadsheet.Setup(x => x.GetString(2, 15)).Returns("rhoncus");
        this.spreadsheet.Setup(x => x.GetString(2, 16)).Returns("erat");
        this.spreadsheet.Setup(x => x.GetString(2, 17)).Returns("metus");
    }

    [Fact]
    public void ReadRow_ReturnsAPaymentWithTheCorrectProperties()
    {
        var paymentReader = new PaymentReader(this.spreadsheet.Object);

        var payment = paymentReader.ReadRow(2);

        var expected = new Payment
        {
            PaymentReference = "lorem",
            InvoiceNumber = "ipsum",
            Date = "dolor",
            Amount = "sit",
            PaymentAmount = "amet",
            ShortPayIndicator = "consectetur",
            PaymentNote = "adipiscing",
            AutoDebitIndicator = "elit",
            InvoiceBalance = "Quisque",
            PaymentTimestamp = "porttitor",
            InvoiceDivision = "porttitor",
            InvoiceDivisionNumber = "tortor",
            PayToBankAccount = "non",
            CustomerId = "auctor",
            Status = "Aliquam",
            PaymentSource = "rhoncus",
            PaymentCode = "erat",
            PaymentDescription = "metus",
        };

        Assert.Equal(expected, payment);
    }
}
