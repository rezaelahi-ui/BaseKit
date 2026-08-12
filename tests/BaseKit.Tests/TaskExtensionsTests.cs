using System;
using System.Threading.Tasks;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class TaskExtensionsTests
{
    [Fact]
    public async Task WithTimeout_Generic_ReturnsResult_WhenCompletesInTime()
    {
        var task = Task.FromResult(42);
        var result = await task.WithTimeout(TimeSpan.FromSeconds(2));
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task WithTimeout_Generic_Throws_WhenExceedsTimeout()
    {
        var task = Task.Delay(500).ContinueWith(_ => 1);
        await Assert.ThrowsAsync<TimeoutException>(() => task.WithTimeout(TimeSpan.FromMilliseconds(50)));
    }

    [Fact]
    public async Task WithTimeout_NonGeneric_CompletesNormally_WhenWithinTimeout()
    {
        var task = Task.Delay(10);
        await task.WithTimeout(TimeSpan.FromSeconds(2));
        Assert.True(task.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task WithTimeout_NonGeneric_Throws_WhenExceedsTimeout()
    {
        var task = Task.Delay(500);
        await Assert.ThrowsAsync<TimeoutException>(() => task.WithTimeout(TimeSpan.FromMilliseconds(50)));
    }
}
