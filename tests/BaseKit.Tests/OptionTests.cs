using System;
using BaseKit.Common;

namespace BaseKit.Tests;

public class OptionTests
{
    [Fact]
    public void Some_HasValue_ReturnsTrue()
    {
        var option = Option<int>.Some(42);

        Assert.True(option.HasValue);
        Assert.Equal(42, option.GetValueOrThrow());
    }

    [Fact]
    public void None_HasValue_ReturnsFalse()
    {
        var option = Option<int>.None();
        Assert.False(option.HasValue);
    }

    [Fact]
    public void GetValueOrThrow_Throws_WhenNone()
    {
        var option = Option<string>.None();
        Assert.Throws<InvalidOperationException>(() => option.GetValueOrThrow());
    }

    [Fact]
    public void GetValueOrDefault_ReturnsValue_WhenSome()
    {
        var option = Option<int>.Some(5);
        Assert.Equal(5, option.GetValueOrDefault(99));
    }

    [Fact]
    public void GetValueOrDefault_ReturnsDefault_WhenNone()
    {
        var option = Option<int>.None();
        Assert.Equal(99, option.GetValueOrDefault(99));
    }

    [Fact]
    public void TryGetValue_ReturnsTrueAndValue_WhenSome()
    {
        var option = Option<string>.Some("ok");
        var success = option.TryGetValue(out var value);

        Assert.True(success);
        Assert.Equal("ok", value);
    }

    [Fact]
    public void TryGetValue_ReturnsFalse_WhenNone()
    {
        var option = Option<string>.None();
        var success = option.TryGetValue(out _);
        Assert.False(success);
    }

    [Fact]
    public void Match_InvokesSome_WhenHasValue()
    {
        var option = Option<int>.Some(10);
        var result = option.Match(v => v * 2, () => -1);
        Assert.Equal(20, result);
    }

    [Fact]
    public void Match_InvokesNone_WhenNoValue()
    {
        var option = Option<int>.None();
        var result = option.Match(v => v * 2, () => -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void FromNullable_ReturnsSome_WhenValueNotNull()
    {
        var option = Option<string>.FromNullable("value");
        Assert.True(option.HasValue);
        Assert.Equal("value", option.GetValueOrThrow());
    }

    [Fact]
    public void FromNullable_ReturnsNone_WhenValueIsNull()
    {
        string? value = null;
        var option = Option<string>.FromNullable(value);
        Assert.False(option.HasValue);
    }
}
