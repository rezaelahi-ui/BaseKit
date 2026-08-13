using System.ComponentModel.DataAnnotations;
using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class CompareToAttributeTests
{
    private class SampleDto
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    private static ValidationContext CreateContext(object instance, string memberName)
        => new(instance) { MemberName = memberName };

    [Theory]
    [InlineData(5, 10, CompareType.LessThan, true)]
    [InlineData(10, 10, CompareType.LessThan, false)]
    [InlineData(10, 10, CompareType.LessThanOrEqual, true)]
    [InlineData(15, 10, CompareType.GreaterThan, true)]
    [InlineData(10, 10, CompareType.Equal, true)]
    [InlineData(5, 10, CompareType.Equal, false)]
    [InlineData(5, 10, CompareType.NotEqual, true)]
    public void GetValidationResult_ComparesAgainstOtherProperty(int start, int end, CompareType comparison, bool expectedValid)
    {
        var dto = new SampleDto { Start = start, End = end };
        var attr = new CompareToAttribute(nameof(SampleDto.End), comparison);
        var context = CreateContext(dto, nameof(SampleDto.Start));

        var result = attr.GetValidationResult(dto.Start, context);

        Assert.Equal(expectedValid, result == ValidationResult.Success);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenValueIsNull()
    {
        var dto = new SampleDto();
        var attr = new CompareToAttribute(nameof(SampleDto.End), CompareType.LessThan);
        var context = CreateContext(dto, nameof(SampleDto.Start));

        var result = attr.GetValidationResult(null, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenOtherPropertyNotFound()
    {
        var dto = new SampleDto { Start = 1 };
        var attr = new CompareToAttribute("NotExist", CompareType.LessThan);
        var context = CreateContext(dto, nameof(SampleDto.Start));

        var result = attr.GetValidationResult(1, context);

        Assert.NotNull(result);
    }
}
