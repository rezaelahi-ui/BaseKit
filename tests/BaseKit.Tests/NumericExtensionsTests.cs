using BaseKit.Extensions;

namespace BaseKit.Tests;

public class NumericExtensionsTests
{
    [Theory]
    [InlineData(1234567, "1,234,567")]
    [InlineData(0, "0")]
    [InlineData(-42, "-42")]
    public void ToSeparatedString_Int(int input, string expected)
    {
        Assert.Equal(expected, input.ToSeparatedString());
    }

    [Theory]
    [InlineData(1234567L, "1,234,567")]
    [InlineData(0L, "0")]
    public void ToSeparatedString_Long(long input, string expected)
    {
        Assert.Equal(expected, input.ToSeparatedString());
    }

    [Theory]
    [InlineData(1234.5, "1,234.5")]
    [InlineData(0.0, "0")]
    public void ToSeparatedString_Decimal(decimal input, string expected)
    {
        Assert.Equal(expected, input.ToSeparatedString());
    }

    [Theory]
    [InlineData(1234.5, "1,234.5")]
    public void ToSeparatedString_Double(double input, string expected)
    {
        Assert.Equal(expected, input.ToSeparatedString());
    }

    [Theory]
    [InlineData(1234567L, "ریال", "۱,۲۳۴,۵۶۷ ریال")]
    [InlineData(100L, "", "۱۰۰")]
    public void ToPersianCurrency_Long(long input, string unit, string expected)
    {
        Assert.Equal(expected, input.ToPersianCurrency(unit));
    }

    [Theory]
    [InlineData(1234567, "ریال", "۱,۲۳۴,۵۶۷ ریال")]
    public void ToPersianCurrency_Decimal(decimal input, string unit, string expected)
    {
        Assert.Equal(expected, input.ToPersianCurrency(unit));
    }

    [Theory]
    [InlineData(0, "صفر")]
    [InlineData(15, "پانزده")]
    [InlineData(21, "بیست و یک")]
    [InlineData(100, "صد")]
    [InlineData(1234, "یک هزار و دویست و سی و چهار")]
    [InlineData(1000000, "یک میلیون")]
    [InlineData(-5, "منفی پنج")]
    public void ToPersianWords(long input, string expected)
    {
        Assert.Equal(expected, input.ToPersianWords());
    }
}
