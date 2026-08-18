using System;
using System.Collections.Generic;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetOrDefault_ReturnsValue_WhenKeyExists()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        Assert.Equal(1, dict.GetOrDefault("a", -1));
    }

    [Fact]
    public void GetOrDefault_ReturnsDefault_WhenKeyMissing()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        Assert.Equal(-1, dict.GetOrDefault("missing", -1));
    }

    [Fact]
    public void GetOrDefault_ReturnsDefaultOfT_WhenNotSpecified()
    {
        var dict = new Dictionary<string, int>();
        Assert.Equal(0, dict.GetOrDefault("missing"));
    }

    [Fact]
    public void GetOrDefault_Throws_WhenDictionaryNull()
    {
        IReadOnlyDictionary<string, int>? dict = null;
        Assert.Throws<ArgumentNullException>(() => dict!.GetOrDefault("a", -1));
    }
}
