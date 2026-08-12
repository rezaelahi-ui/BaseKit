using System;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class ReflectionExtensionsTests
{
    public class Dto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Fact]
    public void Clone_CreatesIndependentCopyWithSameValues()
    {
        var original = new Dto { Name = "Ali", Age = 30 };

        var clone = original.Clone();

        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.Age, clone.Age);
        Assert.NotSame(original, clone);
    }

    [Fact]
    public void Clone_Throws_WhenSourceIsNull()
    {
        Dto? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.Clone());
    }

    [Fact]
    public void ToDictionary_ReturnsPropertyNamesAndValues()
    {
        var source = new Dto { Name = "Reza", Age = 25 };

        var dict = source.ToDictionary();

        Assert.Equal("Reza", dict["Name"]);
        Assert.Equal(25, dict["Age"]);
    }

    [Fact]
    public void ToDictionary_Throws_WhenSourceIsNull()
    {
        object? source = null;
        Assert.Throws<ArgumentNullException>(() => source!.ToDictionary());
    }
}
