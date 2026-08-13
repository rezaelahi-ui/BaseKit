using System.ComponentModel.DataAnnotations;
using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class RequiredIfAttributeTests
{
    private class OrderDto
    {
        public bool HasDiscount { get; set; }
        public decimal? DiscountAmount { get; set; }
    }

    private static ValidationContext CreateContext(object instance, string memberName)
        => new(instance) { MemberName = memberName };

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenConditionMetAndValueMissing()
    {
        var dto = new OrderDto { HasDiscount = true, DiscountAmount = null };
        var attr = new RequiredIfAttribute(nameof(OrderDto.HasDiscount), true);
        var context = CreateContext(dto, nameof(OrderDto.DiscountAmount));

        var result = attr.GetValidationResult(dto.DiscountAmount, context);

        Assert.NotNull(result);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenConditionMetAndValueProvided()
    {
        var dto = new OrderDto { HasDiscount = true, DiscountAmount = 10 };
        var attr = new RequiredIfAttribute(nameof(OrderDto.HasDiscount), true);
        var context = CreateContext(dto, nameof(OrderDto.DiscountAmount));

        var result = attr.GetValidationResult(dto.DiscountAmount, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsSuccess_WhenConditionNotMet()
    {
        var dto = new OrderDto { HasDiscount = false, DiscountAmount = null };
        var attr = new RequiredIfAttribute(nameof(OrderDto.HasDiscount), true);
        var context = CreateContext(dto, nameof(OrderDto.DiscountAmount));

        var result = attr.GetValidationResult(dto.DiscountAmount, context);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void GetValidationResult_ReturnsFailure_WhenOtherPropertyNotFound()
    {
        var dto = new OrderDto();
        var attr = new RequiredIfAttribute("NotExist", true);
        var context = CreateContext(dto, nameof(OrderDto.DiscountAmount));

        var result = attr.GetValidationResult(null, context);

        Assert.NotNull(result);
    }
}
