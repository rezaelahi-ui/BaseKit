using System;
using System.Collections.Generic;
using System.Linq;
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

    [Fact]
    public async Task WhenAllSafe_CompletesNormally_WhenAllTasksSucceed()
    {
        var tasks = new[] { Task.Delay(1), Task.Delay(1), Task.Delay(1) };
        await tasks.WhenAllSafe();
        Assert.All(tasks, t => Assert.True(t.IsCompletedSuccessfully));
    }

    [Fact]
    public async Task WhenAllSafe_Throws_AggregateException_WithAllFailures()
    {
        var tasks = new List<Task>
        {
            Task.Run(() => throw new InvalidOperationException("خطای اول")),
            Task.Run(() => throw new ArgumentException("خطای دوم")),
            Task.Delay(1),
        };

        var ex = await Assert.ThrowsAsync<AggregateException>(() => tasks.WhenAllSafe());
        Assert.Equal(2, ex.InnerExceptions.Count);
        Assert.Contains(ex.InnerExceptions, e => e is InvalidOperationException);
        Assert.Contains(ex.InnerExceptions, e => e is ArgumentException);
    }

    [Fact]
    public async Task WhenAllSafe_Generic_ReturnsAllResults()
    {
        var tasks = new[] { Task.FromResult(1), Task.FromResult(2), Task.FromResult(3) };
        var results = await tasks.WhenAllSafe();
        Assert.Equal(new[] { 1, 2, 3 }, results);
    }

    [Fact]
    public async Task WhenAllSafe_Throws_WhenSourceNull()
    {
        IEnumerable<Task>? tasks = null;
        await Assert.ThrowsAsync<ArgumentNullException>(() => tasks!.WhenAllSafe());
    }
}
