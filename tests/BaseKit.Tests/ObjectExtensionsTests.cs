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
}
