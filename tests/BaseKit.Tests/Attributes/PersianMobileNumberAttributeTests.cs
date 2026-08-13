using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class PersianMobileNumberAttributeTests
{
    [Theory]
    [InlineData("09123456789", true)]
    [InlineData("08123456789", false)]
    [InlineData(null, true)] // الزامی‌بودن مسئولیت [Required] است
    public void IsValid_ValidatesMobileNumber(object? value, bool expected)
    {
        var attr = new PersianMobileNumberAttribute();
        Assert.Equal(expected, attr.IsValid(value));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueIsNotString()
    {
        var attr = new PersianMobileNumberAttribute();
        Assert.False(attr.IsValid(9123456789));
    }
}
