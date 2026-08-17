using System;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ObjectExtensionsTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("value", false)]
    public void IsEmpty(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsEmpty());
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("value", true)]
    public void IsNotEmpty(string? input, bool expected)
    {
        Assert.Equal(expected, input.IsNotEmpty());
    }

    [Theory]
    [InlineData(2, new[] { 1, 2, 3 }, true)]
    [InlineData(5, new[] { 1, 2, 3 }, false)]
    [InlineData(1, new int[0], false)]
    public void In(int value, int[] values, bool expected)
    {
        Assert.Equal(expected, value.In(values));
    }

    [Fact]
    public void In_Throws_WhenValuesNull()
    {
        int[]? values = null;
        Assert.Throws<ArgumentNullException>(() => 1.In(values!));
    }

    [Fact]
    public void Tap_InvokesAction_AndReturnsSameInstance()
    {
        var invokedWith = -1;
        var result = 42.Tap(x => invokedWith = x);

        Assert.Equal(42, invokedWith);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Tap_Throws_WhenActionNull()
    {
        Action<int>? action = null;
        Assert.Throws<ArgumentNullException>(() => 1.Tap(action!));
    }
}
