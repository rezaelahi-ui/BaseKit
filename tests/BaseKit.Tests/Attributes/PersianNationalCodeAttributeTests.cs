using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class PersianNationalCodeAttributeTests
{
    [Theory]
    [InlineData("0499370899", true)]
    [InlineData("0499370890", false)]
    [InlineData(null, true)] // الزامی‌بودن مسئولیت [Required] است
    public void IsValid_ValidatesNationalCode(object? value, bool expected)
    {
        var attr = new PersianNationalCodeAttribute();
        Assert.Equal(expected, attr.IsValid(value));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueIsNotString()
    {
        var attr = new PersianNationalCodeAttribute();
        Assert.False(attr.IsValid(499370899));
    }
}
