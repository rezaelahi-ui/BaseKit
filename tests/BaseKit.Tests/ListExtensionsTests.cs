using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ListExtensionsTests
{
    public static IEnumerable<object[]> IListCases()
    {
        yield return new object[] { null!, true };
        yield return new object[] { new List<int>(), true };
        yield return new object[] { new List<int> { 1 }, false };
    }

    [Theory]
    [MemberData(nameof(IListCases))]
    public void IsEmpty_IList(IList input, bool expected)
    {
        Assert.Equal(expected, input.IsEmpty());
    }

    public static IEnumerable<object[]> IEnumerableCases()
    {
        yield return new object[] { null!, true };
        yield return new object[] { Enumerable.Empty<int>(), true };
        yield return new object[] { new[] { 1, 2 }, false };
    }

    [Theory]
    [MemberData(nameof(IEnumerableCases))]
    public void IsEmpty_IEnumerable(IEnumerable input, bool expected)
    {
        Assert.Equal(expected, input.IsEmpty());
    }

    [Theory]
    [MemberData(nameof(IEnumerableCases))]
    public void IsNotEmpty(IEnumerable input, bool isEmptyExpected)
    {
        Assert.Equal(!isEmptyExpected, input.IsNotEmpty());
    }

    [Theory]
    [MemberData(nameof(JoinedNamesCases))]
    public void GetJoinedNames(List<Person> items, string expected)
    {
        Assert.Equal(expected, items.GetJoinedNames(p => p.Name));
    }

    public static IEnumerable<object[]> JoinedNamesCases()
    {
        yield return new object[] { new List<Person> { new("Ali"), new("Reza") }, "Ali,Reza" };
        yield return new object[] { new List<Person>(), string.Empty };
    }

    [Theory]
    [InlineData(new[] { 1 }, new[] { 1, 2 }, true)]      // تعداد متفاوت
    [InlineData(new[] { 1, 2 }, new[] { 1, 3 }, true)]   // محتوای متفاوت
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 }, false)]  // همون آیتم‌ها با ترتیب متفاوت
    public void HasChanges(int[] oldItems, int[] newItems, bool expected)
    {
        Assert.Equal(expected, oldItems.ToList().HasChanges(newItems.ToList()));
    }

    public record Person(string Name);

    [Theory]
    [InlineData(0, "a")]
    [InlineData(2, "c")]
    public void GetOrDefault_Index_ReturnsItem_WhenInRange(int index, string expected)
    {
        var list = new List<string> { "a", "b", "c" };
        Assert.Equal(expected, list.GetOrDefault(index, "fallback"));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(-1)]
    public void GetOrDefault_Index_ReturnsDefault_WhenOutOfRange(int index)
    {
        var list = new List<string> { "a", "b", "c" };
        Assert.Equal("fallback", list.GetOrDefault(index, "fallback"));
    }

    [Fact]
    public void GetOrDefault_Index_Throws_WhenListNull()
    {
        List<int>? list = null;
        Assert.Throws<ArgumentNullException>(() => list!.GetOrDefault(0, -1));
    }
}
