using System;
using BaseKit.Exceptions;

namespace BaseKit.Tests;

public class AlertExceptionTests
{
    [Theory]
    [InlineData("پیام خطا")]
    [InlineData("")]
    public void Constructor_SetsMessage(string message)
    {
        var ex = new AlertException(message);
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void Constructor_SetsMessageAndInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new AlertException("پیام خطا", inner);

        Assert.Equal("پیام خطا", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }
}
