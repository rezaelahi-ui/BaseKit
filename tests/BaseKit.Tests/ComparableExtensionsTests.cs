using System;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ComparableExtensionsTests
{
    [Theory]
    [InlineData(5, 1, 10, true)]
    [InlineData(1, 1, 10, true)]  // مرز پایین، شامل بازه
    [InlineData(10, 1, 10, true)] // مرز بالا، شامل بازه
    [InlineData(0, 1, 10, false)]
    [InlineData(11, 1, 10, false)]
    public void Between_Int(int value, int min, int max, bool expected)
    {
        Assert.Equal(expected, value.Between(min, max));
    }

    [Fact]
    public void Between_DateTime_ReturnsTrue_WhenWithinRange()
    {
        var value = new DateTime(2023, 6, 15);
        Assert.True(value.Between(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31)));
    }

    [Fact]
    public void Between_String_ComparesOrdinally()
    {
        Assert.True("m".Between("a", "z"));
        Assert.False("z".Between("a", "m"));
    }

    [Fact]
    public void Between_ReturnsFalse_WhenMinGreaterThanMax()
    {
        Assert.False(5.Between(10, 1));
    }
}
