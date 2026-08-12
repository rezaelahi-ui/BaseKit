using System;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ExceptionExtensionsTests
{
    [Theory]
    [InlineData(" <- ", "outer <- inner")]
    [InlineData(" | ", "outer | inner")]
    public void GetFullMessage_JoinsOuterAndInnerMessages(string separator, string expected)
    {
        var inner = new InvalidOperationException("inner");
        var outer = new Exception("outer", inner);

        Assert.Equal(expected, outer.GetFullMessage(separator));
    }

    [Fact]
    public void GetFullMessage_ReturnsSingleMessage_WhenNoInnerException()
    {
        var ex = new Exception("only message");
        Assert.Equal("only message", ex.GetFullMessage());
    }

    [Fact]
    public void GetFullMessage_Throws_WhenNull()
    {
        Exception? ex = null;
        Assert.Throws<ArgumentNullException>(() => ex!.GetFullMessage());
    }
}
