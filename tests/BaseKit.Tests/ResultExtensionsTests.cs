using System;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToResult_ReturnsSuccess_WhenFuncCompletes()
    {
        Func<int> func = () => 42;
        var result = func.ToResult();

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ToResult_ReturnsFailure_WhenFuncThrows()
    {
        Func<int> func = () => throw new InvalidOperationException("boom");
        var result = func.ToResult();

        Assert.True(result.IsFailure);
        Assert.Equal("boom", result.Error);
    }

    [Fact]
    public void ToResult_Throws_WhenFuncNull()
    {
        Func<int>? func = null;
        Assert.Throws<ArgumentNullException>(() => func!.ToResult());
    }
}
