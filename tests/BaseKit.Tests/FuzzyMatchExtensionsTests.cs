using System;
using System.Linq;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class FuzzyMatchExtensionsTests
{
    [Theory]
    [InlineData("kitten", "sitting", 3)]
    [InlineData("abc", "abc", 0)]
    [InlineData("", "abc", 3)]
    [InlineData("abc", "", 3)]
    [InlineData("خراسان جنوبی", "خوراسان جنوبی", 1)] // فقط یک حرف «و» اضافه شده
    public void LevenshteinDistance_ComputesMinimumEditOperations(string source, string target, int expected)
    {
        Assert.Equal(expected, source.LevenshteinDistance(target));
    }

    [Theory]
    [InlineData(null)]
    public void LevenshteinDistance_Throws_WhenSourceIsNull(string? source)
    {
        Assert.Throws<ArgumentNullException>(() => source!.LevenshteinDistance("x"));
    }

    [Fact]
    public void LevenshteinDistance_Throws_WhenTargetIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => "x".LevenshteinDistance(null!));
    }

    [Theory]
    [InlineData("abc", "abc", 1.0)]
    [InlineData("", "", 1.0)]
    [InlineData("abc", "abd", 0.6666666666666667)]
    public void SimilarityTo_ReturnsRatioBetweenZeroAndOne(string source, string target, double expected)
    {
        Assert.Equal(expected, source.SimilarityTo(target), precision: 10);
    }

    [Theory]
    [InlineData("خراسان جنوبی", "خوراسان جنوبی", 0.8, true)]
    [InlineData("Tehran", "Computer", 0.8, false)]
    public void IsSimilarTo_ComparesAgainstThreshold(string source, string target, double threshold, bool expected)
    {
        Assert.Equal(expected, source.IsSimilarTo(target, threshold));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void IsSimilarTo_Throws_WhenThresholdOutOfRange(double threshold)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "a".IsSimilarTo("b", threshold));
    }

    [Fact]
    public void FindBestMatch_ReturnsClosestItem()
    {
        var cities = new[] { "Tehran", "Mashhad", "Shiraz" };
        Assert.Equal("Tehran", cities.FindBestMatch("Tehren"));
    }

    [Fact]
    public void FindBestMatch_ReturnsNull_WhenSourceEmpty()
    {
        Assert.Null(Array.Empty<string>().FindBestMatch("x"));
    }

    [Fact]
    public void FindBestMatch_Throws_WhenSourceIsNull()
    {
        string[]? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.FindBestMatch("x"));
    }

    [Fact]
    public void FindSimilar_ReturnsOnlyItemsAboveThreshold_OrderedByScoreDescending()
    {
        var cities = new[] { "خراسان جنوبی", "خراسان رضوی", "تهران" };

        var result = cities.FindSimilar("خوراسان جنوبی", 0.8).ToList();

        Assert.Single(result);
        Assert.Equal("خراسان جنوبی", result[0].Item);
        Assert.True(result[0].Score >= 0.8);
    }

    [Fact]
    public void FindSimilar_OrdersMultipleMatchesByScoreDescending()
    {
        var items = new[] { "xyz", "abd", "abc" };

        var result = items.FindSimilar("abc", threshold: 0).ToList();

        Assert.Equal(new[] { "abc", "abd", "xyz" }, result.Select(r => r.Item));
        Assert.True(result[0].Score > result[1].Score);
        Assert.True(result[1].Score > result[2].Score);
    }

    [Fact]
    public void FindSimilar_Throws_WhenSourceIsNull()
    {
        string[]? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.FindSimilar("x").ToList());
    }
}
