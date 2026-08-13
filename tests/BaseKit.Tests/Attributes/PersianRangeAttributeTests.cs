using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class PersianRangeAttributeTests
{
    [Theory]
    [InlineData(5, 1, 10)]
    [InlineData(1, 1, 10)]
    [InlineData(10, 1, 10)]
    public void IsValid_ReturnsTrue_WhenWithinRange(double value, double min, double max)
    {
        var attr = new PersianRangeAttribute(min, max);
        Assert.True(attr.IsValid(value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void IsValid_ReturnsFalse_WhenOutsideRange(double value)
    {
        var attr = new PersianRangeAttribute(1, 10);
        Assert.False(attr.IsValid(value));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsNull()
    {
        // الزامی‌بودن مسئولیت [Required] است، نه این attribute
        var attr = new PersianRangeAttribute(1, 10);
        Assert.True(attr.IsValid(null));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueNotConvertibleToNumber()
    {
        var attr = new PersianRangeAttribute(1, 10);
        Assert.False(attr.IsValid("not-a-number"));
    }

    [Fact]
    public void ErrorMessage_UsesCustomMessage_WhenProvided()
    {
        var attr = new PersianRangeAttribute(1, 10, "پیام سفارشی");
        Assert.Equal("پیام سفارشی", attr.ErrorMessage);
    }

    [Fact]
    public void ErrorMessage_UsesDefaultMessage_WhenNotProvided()
    {
        var attr = new PersianRangeAttribute(1, 10);
        Assert.Contains("1", attr.ErrorMessage);
        Assert.Contains("10", attr.ErrorMessage);
    }
}
