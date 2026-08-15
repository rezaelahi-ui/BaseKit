using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ValidationExtensionsTests
{
    [Theory]
    [InlineData("0499370899", true)]   // چک‌دیجیت معتبر
    [InlineData("0499370890", false)]  // چک‌دیجیت نامعتبر
    [InlineData("0000000000", false)]  // ارقام تکراری
    [InlineData("12345", false)]       // طول نامعتبر
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidNationalCode(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidNationalCode());
    }

    [Theory]
    [InlineData("09123456789", true)]
    [InlineData("+989123456789", true)]
    [InlineData("00989123456789", true)]
    [InlineData("9123456789", true)]
    [InlineData("08123456789", false)]  // پیش‌شماره اشتباه
    [InlineData("0912345678", false)]   // یک رقم کم
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidMobileNumber(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidMobileNumber());
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("a@b.co", true)]
    [InlineData("invalid-email", false)]
    [InlineData("@missinguser.com", false)]
    [InlineData("user@domain", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidEmail(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidEmail());
    }

    [Theory]
    [InlineData("DE89370400440532013000", true)]
    [InlineData("GB82WEST12345698765432", true)]
    [InlineData("DE89370400440532013001", false)] // چک‌دیجیت اشتباه
    [InlineData("not-an-iban", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidIban(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidIban());
    }

    [Theory]
    [InlineData("1234567890", true)]
    [InlineData("0000000000", false)] // ارقام تکراری
    [InlineData("12345", false)]      // طول نامعتبر
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidPostalCode(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidPostalCode());
    }

    [Theory]
    [InlineData("12345678918", true)]  // چک‌دیجیت معتبر (طبق ضرایب الگوریتم)
    [InlineData("12345678910", false)] // چک‌دیجیت نامعتبر
    [InlineData("123", false)]         // طول نامعتبر
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidLegalNationalId(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidLegalNationalId());
    }

    [Theory]
    [InlineData("6037990000000006", true)]  // Luhn معتبر
    [InlineData("6037990000000007", false)] // Luhn نامعتبر
    [InlineData("12345", false)]            // طول نامعتبر
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidCardNumber(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidCardNumber());
    }

    [Theory]
    [InlineData("6037990000000006", "بانک ملی ایران")]
    [InlineData("6037-9900-0000-0006", "بانک ملی ایران")] // با جداکننده
    [InlineData("9999990000000006", null)]                // BIN ناشناخته
    [InlineData(null, null)]
    [InlineData("", null)]
    public void GetBankName_FromCardNumber(string? input, string? expected)
    {
        Assert.Equal(expected, input.GetBankName());
    }

    [Theory]
    [InlineData("IR120170000000000000000000", "بانک ملی ایران")]
    [InlineData("IR129990000000000000000000", null)] // کد بانک ناشناخته
    [InlineData("DE120170000000000000000000", null)] // پیشوند غیر IR
    [InlineData(null, null)]
    [InlineData("", null)]
    public void GetBankNameFromIban(string? input, string? expected)
    {
        Assert.Equal(expected, input.GetBankNameFromIban());
    }

    [Theory]
    [InlineData("12ب34567", true)]
    [InlineData("12 ب 34567", true)] // با فاصله
    [InlineData("1ب34567", false)]   // فرمت نامعتبر
    [InlineData("12ژ34567", false)]  // حرف خارج از فهرست پلاک
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsValidPlateNumber(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsValidPlateNumber());
    }
}
