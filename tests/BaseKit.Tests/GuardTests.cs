using System;
using BaseKit.Guards;

namespace BaseKit.Tests;

public class GuardTests
{
    [Fact]
    public void Against_Null_ReturnsValue_WhenNotNull()
    {
        var value = "hello";
        Assert.Equal(value, Guard.Against.Null(value, nameof(value)));
    }

    [Fact]
    public void Against_Null_Throws_WhenNull()
    {
        string? value = null;
        Assert.Throws<ArgumentNullException>(() => Guard.Against.Null(value, nameof(value)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Against_Empty_Throws_WhenEmptyOrNull(string? value)
    {
        Assert.Throws<ArgumentException>(() => Guard.Against.Empty(value, nameof(value)));
    }

    [Fact]
    public void Against_Empty_ReturnsValue_WhenNotEmpty()
    {
        Assert.Equal("value", Guard.Against.Empty("value", "param"));
    }

    [Theory]
    [InlineData(-1, true)]
    [InlineData(0, false)]
    [InlineData(5, false)]
    public void Against_Negative(int value, bool shouldThrow)
    {
        if (shouldThrow)
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Against.Negative(value, nameof(value)));
        else
            Assert.Equal(value, Guard.Against.Negative(value, nameof(value)));
    }

    [Theory]
    [InlineData(5, 1, 10, false)]
    [InlineData(0, 1, 10, true)]
    [InlineData(11, 1, 10, true)]
    public void Against_OutOfRange(int value, int min, int max, bool shouldThrow)
    {
        if (shouldThrow)
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Against.OutOfRange(value, min, max, nameof(value)));
        else
            Assert.Equal(value, Guard.Against.OutOfRange(value, min, max, nameof(value)));
    }
}
