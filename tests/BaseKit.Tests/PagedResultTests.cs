using System;
using System.Collections.Generic;
using BaseKit.Common;

namespace BaseKit.Tests;

public class PagedResultTests
{
    [Theory]
    [InlineData(10, 3, 4)]  // ceil(10/3) = 4
    [InlineData(9, 3, 3)]
    [InlineData(0, 5, 0)]
    public void TotalPages_ComputedFromTotalCountAndPageSize(int totalCount, int pageSize, int expectedTotalPages)
    {
        var result = new PagedResult<int>(new List<int>(), 1, pageSize, totalCount);
        Assert.Equal(expectedTotalPages, result.TotalPages);
    }

    [Theory]
    [InlineData(1, 4, false, true)]  // صفحه اول: قبلی نداره، بعدی داره
    [InlineData(2, 4, true, true)]   // صفحه میانی: هم قبلی هم بعدی داره
    [InlineData(4, 4, true, false)]  // صفحه آخر: قبلی داره، بعدی نداره
    public void HasPreviousAndNextPage_ReflectPositionWithinTotalPages(
        int pageNumber, int totalPages, bool expectedHasPrevious, bool expectedHasNext)
    {
        // totalPages=4 با pageSize=1 یعنی totalCount=4
        var result = new PagedResult<int>(new List<int> { 1 }, pageNumber, 1, totalPages);

        Assert.Equal(expectedHasPrevious, result.HasPreviousPage);
        Assert.Equal(expectedHasNext, result.HasNextPage);
    }

    [Fact]
    public void Constructor_Throws_WhenItemsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new PagedResult<int>(null!, 1, 10, 0));
    }

    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(1, 10, -1)]
    public void Constructor_Throws_WhenArgumentsOutOfRange(int pageNumber, int pageSize, int totalCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PagedResult<int>(new List<int>(), pageNumber, pageSize, totalCount));
    }
}
