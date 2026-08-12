using System;
using System.ComponentModel;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class EnumExtensionsTests
{
    public enum Sample
    {
        [Description("مقدار اول")]
        First = 1,
        Second = 2,
    }

    [Theory]
    [InlineData(Sample.First, "مقدار اول")]  // اتریبیوت Description دارد
    [InlineData(Sample.Second, "Second")]     // اتریبیوت ندارد، نام enum برگردانده می‌شود
    public void Humanize(Sample value, string expected)
    {
        Assert.Equal(expected, value.Humanize());
    }

    [Theory]
    [InlineData(Sample.First, 1)]
    [InlineData(Sample.Second, 2)]
    public void ToInt(Sample value, int expected)
    {
        Assert.Equal(expected, value.ToInt());
    }

    [Fact]
    public void ToInt_Throws_WhenNull()
    {
        Enum? value = null;
        Assert.Throws<ArgumentNullException>(() => value!.ToInt());
    }

    [Fact]
    public void GetAllNames_ReturnsHumanizedNamesForAllValues()
    {
        var names = Sample.First.GetAllNames();
        Assert.Equal(new[] { "مقدار اول", "Second" }, names);
    }

    [Fact]
    public void GetAll_ReturnsAllEnumValuesCastToTargetType()
    {
        var values = Sample.First.GetAll<int>();
        Assert.Equal(new[] { 1, 2 }, values);
    }

    [Theory]
    [InlineData(false, 2, "مقدار اول", 1)]
    [InlineData(true, 3, "همه", -1)]
    public void GetDetails(bool withAll, int expectedCount, string expectedFirstName, int expectedFirstValue)
    {
        var details = Sample.First.GetDetails(withAll);

        Assert.Equal(expectedCount, details.Count);
        Assert.Equal(expectedFirstName, details[0].Name);
        Assert.Equal(expectedFirstValue, details[0].Value);
    }

    [Theory]
    [InlineData("First", Sample.First)]
    [InlineData("second", Sample.Second)] // case-insensitive
    public void ToEnum_FromString_ParsesValidNames(string input, Sample expected)
    {
        Assert.Equal(expected, input.ToEnum<Sample>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToEnum_FromString_Throws_WhenEmpty(string? input)
    {
        Assert.Throws<ArgumentNullException>(() => input!.ToEnum<Sample>());
    }

    [Fact]
    public void ToEnum_FromString_Throws_WhenNotDefined()
    {
        Assert.Throws<FormatException>(() => "NotAValue".ToEnum<Sample>());
    }

    [Theory]
    [InlineData(1, Sample.First)]
    [InlineData(2, Sample.Second)]
    public void ToEnum_FromInt_ParsesValidValues(int input, Sample expected)
    {
        Assert.Equal(expected, input.ToEnum<Sample>());
    }

    [Fact]
    public void ToEnum_FromInt_Throws_WhenNotDefined()
    {
        Assert.Throws<FormatException>(() => 99.ToEnum<Sample>());
    }
}
