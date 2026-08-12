using System;
using BaseKit.Common;

namespace BaseKit.Tests;

public class ResultTests
{
    [Fact]
    public void Success_SetsValueAndIsSuccess()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_SetsErrorAndIsFailure()
    {
        var result = Result<int>.Failure("خطا رخ داد");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("خطا رخ داد", result.Error);
    }

    [Fact]
    public void GetValueOrThrow_ReturnsValue_WhenSuccess()
    {
        var result = Result<string>.Success("ok");
        Assert.Equal("ok", result.GetValueOrThrow());
    }

    [Fact]
    public void GetValueOrThrow_Throws_WhenFailure()
    {
        var result = Result<string>.Failure("bad");
        Assert.Throws<InvalidOperationException>(() => result.GetValueOrThrow());
    }
}
