using System;
using System.Collections.Generic;
using System.Linq;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ForEach_InvokesActionForEveryItem()
    {
        var visited = new List<int>();
        new[] { 1, 2, 3 }.ForEach(x => visited.Add(x * 2));

        Assert.Equal(new[] { 2, 4, 6 }, visited);
    }

    [Fact]
    public void ForEach_Throws_WhenSourceNull()
    {
        IEnumerable<int>? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.ForEach(_ => { }));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, 2, new[] { 2, 2, 1 })]
    [InlineData(new[] { 1, 2, 3 }, 5, new[] { 3 })]
    public void ChunkBy_SplitsIntoGroupsOfGivenSize(int[] source, int size, int[] expectedChunkSizes)
    {
        var chunks = source.ChunkBy(size).ToList();
        Assert.Equal(expectedChunkSizes, chunks.Select(c => c.Count));
    }

    [Fact]
    public void ChunkBy_Throws_WhenSizeLessThanOne()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new[] { 1 }.ChunkBy(0).ToList());
    }

    [Fact]
    public void DistinctByKey_KeepsFirstOccurrencePerKey()
    {
        var items = new[] { "apple", "avocado", "banana", "blueberry", "cherry" };
        var result = items.DistinctByKey(x => x[0]).ToList();

        Assert.Equal(new[] { "apple", "banana", "cherry" }, result);
    }

    [Fact]
    public void Page_ReturnsCorrectSlice()
    {
        var items = Enumerable.Range(1, 10);
        var page2 = items.Page(2, 3).ToList();

        Assert.Equal(new[] { 4, 5, 6 }, page2);
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(1, 0)]
    public void Page_Throws_WhenPageNumberOrSizeInvalid(int pageNumber, int pageSize)
    {
        var items = Enumerable.Range(1, 10);
        Assert.Throws<ArgumentOutOfRangeException>(() => items.Page(pageNumber, pageSize).ToList());
    }

    [Fact]
    public void ToPagedResult_ReturnsCorrectPageAndTotals_ForMiddlePage()
    {
        var items = Enumerable.Range(1, 10);
        var result = items.ToPagedResult(pageNumber: 2, pageSize: 3);

        Assert.Equal(new[] { 4, 5, 6 }, result.Items);
        Assert.Equal(10, result.TotalCount);
        Assert.Equal(4, result.TotalPages); // ceil(10/3)
        Assert.True(result.HasPreviousPage);
        Assert.True(result.HasNextPage);
    }

    [Fact]
    public void ToPagedResult_LastPage_HasNoNextPage()
    {
        var items = Enumerable.Range(1, 10);
        var result = items.ToPagedResult(pageNumber: 4, pageSize: 3);

        Assert.Equal(new[] { 10 }, result.Items);
        Assert.False(result.HasNextPage);
        Assert.True(result.HasPreviousPage);
    }

    [Fact]
    public void ToPagedResult_Throws_WhenSourceNull()
    {
        IEnumerable<int>? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.ToPagedResult(1, 10));
    }

    [Fact]
    public void Shuffle_KeepsSameElements_ButMayReorder()
    {
        var list = Enumerable.Range(1, 20).ToList();
        var original = list.ToList();

        list.Shuffle();

        Assert.Equal(original.OrderBy(x => x), list.OrderBy(x => x));
    }

    [Fact]
    public void Shuffle_Throws_WhenListNull()
    {
        IList<int>? list = null;
        Assert.Throws<ArgumentNullException>(() => list!.Shuffle());
    }

    [Fact]
    public void RandomItem_ReturnsItemFromSource()
    {
        var items = new[] { 1, 2, 3, 4, 5 };
        var picked = items.RandomItem();
        Assert.Contains(picked, items);
    }

    [Fact]
    public void RandomItem_Throws_WhenSourceEmpty()
    {
        Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().RandomItem());
    }

    [Fact]
    public void RandomItem_Throws_WhenSourceNull()
    {
        IEnumerable<int>? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.RandomItem());
    }
}
