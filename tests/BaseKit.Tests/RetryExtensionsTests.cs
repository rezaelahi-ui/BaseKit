using System;
using System.Threading.Tasks;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class RetryExtensionsTests
{
    [Fact]
    public async Task RetryAsync_Generic_ReturnsResult_WhenSucceedsBeforeRunningOut()
    {
        var attempts = 0;
        Func<Task<int>> action = () =>
        {
            attempts++;
            if (attempts < 3) throw new InvalidOperationException("temporary failure");
            return Task.FromResult(42);
        };

        var result = await action.RetryAsync(retryCount: 5);

        Assert.Equal(42, result);
        Assert.Equal(3, attempts);
    }

    [Fact]
    public async Task RetryAsync_Generic_Throws_WhenAllAttemptsFail()
    {
        Func<Task<int>> action = () => throw new InvalidOperationException("always fails");

        await Assert.ThrowsAsync<AggregateException>(() => action.RetryAsync(retryCount: 3));
    }

    [Fact]
    public async Task RetryAsync_NonGeneric_CompletesAfterTransientFailures()
    {
        var attempts = 0;
        Func<Task> action = () =>
        {
            attempts++;
            if (attempts < 2) throw new InvalidOperationException("temporary failure");
            return Task.CompletedTask;
        };

        await action.RetryAsync(retryCount: 3);

        Assert.Equal(2, attempts);
    }

    [Fact]
    public async Task RetryAsync_Throws_WhenRetryCountLessThanOne()
    {
        Func<Task<int>> action = () => Task.FromResult(1);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => action.RetryAsync(retryCount: 0));
    }
}
