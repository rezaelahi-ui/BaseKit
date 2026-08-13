using System;
using BaseKit.Common;

namespace BaseKit.Tests;

public class ValidatorTests
{
    private record UserDto(string Name, string Mobile);

    [Fact]
    public void Validate_ReturnsValid_WhenAllRulesPass()
    {
        var dto = new UserDto("Ali", "09123456789");

        var result = Validator<UserDto>.For(dto)
            .Rule(x => !string.IsNullOrEmpty(x.Name), "نام الزامی است")
            .Rule(x => x.Mobile.Length == 11, "موبایل نامعتبر است")
            .Validate();

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_CollectsAllFailedRuleMessages_NotJustFirst()
    {
        var dto = new UserDto("", "123");

        var result = Validator<UserDto>.For(dto)
            .Rule(x => !string.IsNullOrEmpty(x.Name), "نام الزامی است")
            .Rule(x => x.Mobile.Length == 11, "موبایل نامعتبر است")
            .Validate();

        Assert.False(result.IsValid);
        Assert.Equal(new[] { "نام الزامی است", "موبایل نامعتبر است" }, result.Errors);
    }

    [Fact]
    public void Validate_ReturnsOnlyFailingRuleMessages()
    {
        var dto = new UserDto("Ali", "123");

        var result = Validator<UserDto>.For(dto)
            .Rule(x => !string.IsNullOrEmpty(x.Name), "نام الزامی است")
            .Rule(x => x.Mobile.Length == 11, "موبایل نامعتبر است")
            .Validate();

        Assert.False(result.IsValid);
        Assert.Equal(new[] { "موبایل نامعتبر است" }, result.Errors);
    }

    [Fact]
    public void For_Throws_WhenInstanceIsNull()
    {
        UserDto? dto = null;
        Assert.Throws<ArgumentNullException>(() => Validator<UserDto>.For(dto!));
    }

    [Fact]
    public void Rule_Throws_WhenPredicateIsNull()
    {
        var dto = new UserDto("Ali", "09123456789");
        Assert.Throws<ArgumentNullException>(() => Validator<UserDto>.For(dto).Rule(null!, "پیام"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Rule_Throws_WhenMessageIsEmpty(string? message)
    {
        var dto = new UserDto("Ali", "09123456789");
        Assert.Throws<ArgumentNullException>(() => Validator<UserDto>.For(dto).Rule(x => true, message!));
    }
}
