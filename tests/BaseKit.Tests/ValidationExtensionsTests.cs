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
}
