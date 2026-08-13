using System;
using System.ComponentModel.DataAnnotations;
using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class DateRangeAttributeTests
{
    private class EventDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? StartText { get; set; }
        public string? EndText { get; set; }
    }

    private static ValidationContext CreateContext(object instance, string memberName)
        => new(instance) { MemberName = memberName };

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenStartBeforeEnd()
    {
        var dto = new EventDto { StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2024, 1, 10) };
        var attr = new DateRangeAttribute(nameof(EventDto.EndDate));
        var context = CreateContext(dto, nameof(EventDto.StartDate));

        var result = attr.GetValidationResult(dto.StartDate, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenStartAfterEnd()
    {
        var dto = new EventDto { StartDate = new DateTime(2024, 1, 10), EndDate = new DateTime(2024, 1, 1) };
        var attr = new DateRangeAttribute(nameof(EventDto.EndDate));
        var context = CreateContext(dto, nameof(EventDto.StartDate));

        var result = attr.GetValidationResult(dto.StartDate, context);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenEqualAndAllowEqualFalse()
    {
        var date = new DateTime(2024, 1, 1);
        var dto = new EventDto { StartDate = date, EndDate = date };
        var attr = new DateRangeAttribute(nameof(EventDto.EndDate), allowEqual: false);
        var context = CreateContext(dto, nameof(EventDto.StartDate));

        var result = attr.GetValidationResult(dto.StartDate, context);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenEqualAndAllowEqualTrue()
    {
        var date = new DateTime(2024, 1, 1);
        var dto = new EventDto { StartDate = date, EndDate = date };
        var attr = new DateRangeAttribute(nameof(EventDto.EndDate));
        var context = CreateContext(dto, nameof(EventDto.StartDate));

        var result = attr.GetValidationResult(dto.StartDate, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_WorksWithStringDates()
    {
        var dto = new EventDto { StartText = "1402/01/01", EndText = "1402/02/01" };
        var attr = new DateRangeAttribute(nameof(EventDto.EndText));
        var context = CreateContext(dto, nameof(EventDto.StartText));

        var result = attr.GetValidationResult(dto.StartText, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenValueIsNull()
    {
        var dto = new EventDto();
        var attr = new DateRangeAttribute(nameof(EventDto.EndDate));
        var context = CreateContext(dto, nameof(EventDto.StartDate));

        var result = attr.GetValidationResult(null, context);

        Assert.Equal(ValidationResult.Success, result);
    }
}
