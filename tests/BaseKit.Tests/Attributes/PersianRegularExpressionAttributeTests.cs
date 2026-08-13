using System.ComponentModel.DataAnnotations;
using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class PersianRegularExpressionAttributeTests
{
    private class SampleDto
    {
        public string Code { get; set; } = string.Empty; // non-nullable
        public string? OptionalCode { get; set; }         // nullable (reference type)
    }

    private static ValidationContext CreateContext(object instance, string memberName)
        => new(instance) { MemberName = memberName };

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenMatchesPattern()
    {
        var dto = new SampleDto { Code = "AB123" };
        var attr = new PersianRegularExpressionAttribute(@"^[A-Z]{2}\d{3}$", "کد نامعتبر است");
        var context = CreateContext(dto, nameof(SampleDto.Code));

        var result = attr.GetValidationResult(dto.Code, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailureWithCustomMessage_WhenDoesNotMatchPattern()
    {
        var dto = new SampleDto { Code = "invalid" };
        var attr = new PersianRegularExpressionAttribute(@"^[A-Z]{2}\d{3}$", "کد نامعتبر است");
        var context = CreateContext(dto, nameof(SampleDto.Code));

        var result = attr.GetValidationResult(dto.Code, context);

        Assert.NotNull(result);
        Assert.Equal("کد نامعتبر است", result!.ErrorMessage);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenEmptyAndPropertyNotNullable()
    {
        var dto = new SampleDto { Code = "" };
        var attr = new PersianRegularExpressionAttribute(@"^[A-Z]{2}\d{3}$", "کد نامعتبر است");
        var context = CreateContext(dto, nameof(SampleDto.Code));

        var result = attr.GetValidationResult(dto.Code, context);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenEmptyAndPropertyNullable()
    {
        // نکته: تشخیص nullable بودن reference type (string?) فقط روی .NET 6+ کار می‌کند.
        var dto = new SampleDto { OptionalCode = null };
        var attr = new PersianRegularExpressionAttribute(@"^[A-Z]{2}\d{3}$", "کد نامعتبر است");
        var context = CreateContext(dto, nameof(SampleDto.OptionalCode));

        var result = attr.GetValidationResult(dto.OptionalCode, context);

        Assert.Equal(ValidationResult.Success, result);
    }
}
