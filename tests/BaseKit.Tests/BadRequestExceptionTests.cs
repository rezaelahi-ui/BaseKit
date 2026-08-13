using System;
using BaseKit.Exceptions;

namespace BaseKit.Tests;

public class BadRequestExceptionTests
{
    [Theory]
    [InlineData("درخواست نامعتبر است")]
    [InlineData("")]
    public void Constructor_SetsMessage(string message)
    {
        var ex = new BadRequestException(message);
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void Constructor_SetsMessageAndInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new BadRequestException("درخواست نامعتبر است", inner);

        Assert.Equal("درخواست نامعتبر است", ex.Message);
        Assert.Same(inner, ex.InnerException);
    }
}
