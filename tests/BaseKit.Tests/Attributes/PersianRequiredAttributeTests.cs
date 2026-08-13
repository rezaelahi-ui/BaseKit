using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class PersianRequiredAttributeTests
{
    [Fact]
    public void IsValid_ReturnsTrue_WhenValueProvided()
    {
        var attr = new PersianRequiredAttribute();
        Assert.True(attr.IsValid("value"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValid_ReturnsFalse_WhenValueMissing(object? value)
    {
        var attr = new PersianRequiredAttribute();
        Assert.False(attr.IsValid(value));
    }

    [Fact]
    public void ErrorMessage_IncludesFieldName_WhenProvided()
    {
        var attr = new PersianRequiredAttribute("نام");
        Assert.Contains("نام", attr.ErrorMessage);
    }

    [Fact]
    public void ErrorMessage_IsGeneric_WhenNoFieldNameProvided()
    {
        var attr = new PersianRequiredAttribute();
        Assert.Contains("این فیلد", attr.ErrorMessage);
    }
}
