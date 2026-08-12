using System;
using System.Threading;
using BaseKit.Common;

namespace BaseKit.Tests;

public class SimpleCacheTests
{
    [Fact]
    public void Set_ThenTryGet_ReturnsStoredValue()
    {
        var cache = new SimpleCache<string, int>();
        cache.Set("key", 42);

        var found = cache.TryGet("key", out var value);

        Assert.True(found);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGet_ReturnsFalse_WhenKeyMissing()
    {
        var cache = new SimpleCache<string, int>();
        var found = cache.TryGet("missing", out _);

        Assert.False(found);
    }

    [Fact]
    public void TryGet_ReturnsFalse_AfterExpiration()
    {
        var cache = new SimpleCache<string, int>();
        cache.Set("key", 42, TimeSpan.FromMilliseconds(10));

        Thread.Sleep(50);
        var found = cache.TryGet("key", out _);

        Assert.False(found);
    }

    [Fact]
    public void GetOrAdd_ReturnsCachedValue_WithoutCallingFactoryAgain()
    {
        var cache = new SimpleCache<string, int>();
        var factoryCalls = 0;

        int Factory(string _)
        {
            factoryCalls++;
            return 100;
        }

        var first = cache.GetOrAdd("key", Factory);
        var second = cache.GetOrAdd("key", Factory);

        Assert.Equal(100, first);
        Assert.Equal(100, second);
        Assert.Equal(1, factoryCalls);
    }

    [Fact]
    public void Remove_DeletesEntry()
    {
        var cache = new SimpleCache<string, int>();
        cache.Set("key", 1);
        cache.Remove("key");

        Assert.False(cache.TryGet("key", out _));
    }

    [Fact]
    public void Clear_RemovesAllEntries()
    {
        var cache = new SimpleCache<string, int>();
        cache.Set("a", 1);
        cache.Set("b", 2);

        cache.Clear();

        Assert.Equal(0, cache.Count);
    }
}
