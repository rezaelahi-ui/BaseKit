using System;
using System.Net;
using BaseKit.Exceptions;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("123", 123)]
    [InlineData("1,234", 1234)]
    [InlineData("-5", -5)]
    public void ToInt_ParsesValidNumbers(string input, int expected)
    {
        Assert.Equal(expected, input.ToInt());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToInt_Throws_WhenEmpty(string? input)
    {
        Assert.Throws<ArgumentNullException>(() => input!.ToInt());
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.5")]
    public void ToInt_Throws_WhenNotNumeric(string input)
    {
        Assert.Throws<FormatException>(() => input.ToInt());
    }

    [Theory]
    [InlineData("https://example.com/", "https://example.com/")]
    [InlineData("https://example.com", "https://example.com/")]
    [InlineData("HTTP://example.com", "http://example.com/")]
    public void ToUri_ParsesValidHttpUrl(string input, string expected)
    {
        Assert.Equal(expected, input.ToUri().ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToUri_Throws_WhenEmpty(string? input)
    {
        Assert.Throws<ArgumentNullException>(() => input!.ToUri());
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("ftp://example.com")]
    public void ToUri_ThrowsAlertException_WhenMissingHttpPrefix(string input)
    {
        Assert.Throws<AlertException>(() => input.ToUri());
    }

    [Theory]
    [InlineData("192.168.1.1")]
    [InlineData("0.0.0.0")]
    [InlineData("255.255.255.255")]
    public void ToIp_ParsesValidIp(string input)
    {
        Assert.Equal(IPAddress.Parse(input), input.ToIp());
    }

    [Theory]
    [InlineData("999.999.999.999")]
    [InlineData("1.2.3")]
    [InlineData("not-an-ip")]
    public void ToIp_Throws_WhenNotValidIp(string input)
    {
        Assert.Throws<FormatException>(() => input.ToIp());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToIp_Throws_WhenEmpty(string? input)
    {
        Assert.Throws<ArgumentNullException>(() => input!.ToIp());
    }

    [Theory]
    [InlineData("12.5", 12.5)]
    [InlineData("1,234.5", 1234.5)]
    [InlineData("-3.25", -3.25)]
    public void ToDecimal_ParsesValidNumbers(string input, double expected)
    {
        Assert.Equal((decimal)expected, input.ToDecimal());
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.5.6")]
    public void ToDecimal_Throws_WhenNotNumeric(string input)
    {
        Assert.Throws<FormatException>(() => input.ToDecimal());
    }

    [Theory]
    [InlineData("12.5", 12.5)]
    [InlineData("1,234.5", 1234.5)]
    [InlineData("-3.25", -3.25)]
    public void ToDouble_ParsesValidNumbers(string input, double expected)
    {
        Assert.Equal(expected, input.ToDouble());
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.5.6")]
    public void ToDouble_Throws_WhenNotNumeric(string input)
    {
        Assert.Throws<FormatException>(() => input.ToDouble());
    }

    [Theory]
    [InlineData("123456789012", 123456789012)]
    [InlineData("1,234,567", 1234567)]
    [InlineData("-42", -42)]
    public void ToLong_ParsesValidNumbers(string input, long expected)
    {
        Assert.Equal(expected, input.ToLong());
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.5")]
    public void ToLong_Throws_WhenNotNumeric(string input)
    {
        Assert.Throws<FormatException>(() => input.ToLong());
    }

    [Theory]
    [InlineData("1", true)]
    [InlineData("0", false)]
    [InlineData("true", true)]
    [InlineData("False", false)]
    [InlineData("yes", true)]
    [InlineData("No", false)]
    [InlineData("بله", true)]
    [InlineData("خیر", false)]
    public void ToBool_ParsesValidValues(string input, bool expected)
    {
        Assert.Equal(expected, input.ToBool());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToBool_Throws_WhenEmpty(string? input)
    {
        Assert.Throws<ArgumentNullException>(() => input!.ToBool());
    }

    [Theory]
    [InlineData("maybe")]
    [InlineData("2")]
    public void ToBool_Throws_WhenNotRecognized(string input)
    {
        Assert.Throws<FormatException>(() => input.ToBool());
    }

    [Theory]
    [InlineData("123", "۱۲۳")]
    [InlineData("سلام 2024", "سلام ۲۰۲۴")]
    [InlineData("", "")]
    public void ToPersianDigits_ConvertsEnglishDigits(string input, string expected)
    {
        Assert.Equal(expected, input.ToPersianDigits());
    }

    [Theory]
    [InlineData("۱۲۳", "123")]
    [InlineData("٤٥٦", "456")]
    [InlineData("mix۱٢3", "mix123")]
    public void ToEnglishDigits_ConvertsPersianAndArabicDigits(string input, string expected)
    {
        Assert.Equal(expected, input.ToEnglishDigits());
    }

    [Theory]
    [InlineData("علي", "علی")]
    [InlineData("كتاب", "کتاب")]
    [InlineData("سلام", "سلام")]
    public void NormalizeArabicChars_ReplacesArabicLetters(string input, string expected)
    {
        Assert.Equal(expected, input.NormalizeArabicChars());
    }

    [Theory]
    [InlineData("09123456789", "0912***6789")]
    [InlineData("1234", "****")]
    public void Mask_HidesMiddlePortion(string input, string expected)
    {
        Assert.Equal(expected, input.Mask());
    }

    [Theory]
    [InlineData("این یک متن طولانی است", 10, "این یک...")]
    [InlineData("کوتاه", 10, "کوتاه")]
    public void Truncate_ShortensAndPreservesWholeWords(string input, int maxLength, string expected)
    {
        Assert.Equal(expected, input.Truncate(maxLength));
    }

    [Fact]
    public void Truncate_Throws_WhenMaxLengthNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "test".Truncate(-1));
    }
}
