using System.ComponentModel.DataAnnotations;
using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class GreaterThanAttributeTests
{
    [Theory]
    [InlineData(10, 10)]
    [InlineData(10, 15)]
    public void GetValidationResult_ReturnsSuccess_WhenValueMeetsThreshold(double threshold, double value)
    {
        var attr = new GreaterThanAttribute(threshold);
        var context = new ValidationContext(new object());

        var result = attr.GetValidationResult(value, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailureWithMessage_WhenValueBelowThreshold()
    {
        var attr = new GreaterThanAttribute(10, "سن");
        var context = new ValidationContext(new object());

        var result = attr.GetValidationResult(5.0, context);

        Assert.NotNull(result);
        Assert.Contains("سن", result!.ErrorMessage);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenValueIsNull()
    {
        // الزامی‌بودن مسئولیت [Required] است، نه این attribute
        var attr = new GreaterThanAttribute(10);
        var context = new ValidationContext(new object());

        var result = attr.GetValidationResult(null, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenValueNotConvertibleToNumber()
    {
        var attr = new GreaterThanAttribute(10);
        var context = new ValidationContext(new object());

        var result = attr.GetValidationResult("not-a-number", context);

        Assert.NotNull(result);
    }
}
