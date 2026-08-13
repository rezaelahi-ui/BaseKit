using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class IranianIbanAttributeTests
{
    [Theory]
    [InlineData("DE89370400440532013000", true)]
    [InlineData("DE89370400440532013001", false)]
    [InlineData(null, true)] // الزامی‌بودن مسئولیت [Required] است
    public void IsValid_ValidatesIban(object? value, bool expected)
    {
        var attr = new IranianIbanAttribute();
        Assert.Equal(expected, attr.IsValid(value));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenValueIsNotString()
    {
        var attr = new IranianIbanAttribute();
        Assert.False(attr.IsValid(12345));
    }
}
