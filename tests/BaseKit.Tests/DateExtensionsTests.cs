using System;
using BaseKit.Exceptions;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class DateExtensionsTests
{
    [Theory]
    [InlineData("1402/02/01", "1402/01/01", true)]
    [InlineData("1402/01/01", "1402/01/01", false)]
    [InlineData("1402/01/01", "1402/02/01", false)]
    public void IsGreaterThan(string date, string target, bool expected)
    {
        Assert.Equal(expected, date.IsGreaterThan(target));
    }

    [Theory]
    [InlineData("1402/02/01", "1402/01/01", true)]
    [InlineData("1402/01/01", "1402/01/01", true)]
    [InlineData("1402/01/01", "1402/02/01", false)]
    public void IsGreaterOrEqualsThan(string date, string target, bool expected)
    {
        Assert.Equal(expected, date.IsGreaterOrEqualsThan(target));
    }

    [Theory]
    [InlineData("1402/01/01", "1402/02/01", true)]
    [InlineData("1402/01/01", "1402/01/01", false)]
    [InlineData("1402/02/01", "1402/01/01", false)]
    public void IsLowerThan(string date, string target, bool expected)
    {
        Assert.Equal(expected, date.IsLowerThan(target));
    }

    [Theory]
    [InlineData("1402/01/01", "1402/02/01", true)]
    [InlineData("1402/01/01", "1402/01/01", true)]
    [InlineData("1402/02/01", "1402/01/01", false)]
    public void IsLowerOrEqualsThan(string date, string target, bool expected)
    {
        Assert.Equal(expected, date.IsLowerOrEqualsThan(target));
    }

    [Theory]
    [InlineData("1402/01/01", "1402/01/01", true)]
    [InlineData("1402/01/01", "1402/01/02", false)]
    public void IsEqualThan(string date, string target, bool expected)
    {
        Assert.Equal(expected, date.IsEqualThan(target));
    }

    [Theory]
    [InlineData("a,b", 3, "a,b")]
    [InlineData("a,b,c", 1, "a, ...")]
    [InlineData("", 2, "")]
    public void Ellipsis(string input, int count, string expected)
    {
        Assert.Equal(expected, input.Ellipsis(count));
    }

    [Theory]
    [InlineData("value", null, "value")]
    [InlineData("", null, "")]
    [InlineData("", "fallback", "fallback")]
    public void GetSafeValue(string input, string? defaultValue, string expected)
    {
        Assert.Equal(expected, input.GetSafeValue(defaultValue));
    }

    [Theory]
    [InlineData(2023, 3, 21, "1402/01/01")]
    [InlineData(2023, 3, 22, "1402/01/02")]
    public void ToShamsi(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToShamsi());
    }

    [Fact]
    public void ToShamsi_WithAddDay_ShiftsDate()
    {
        var date = new DateTime(2023, 3, 21);
        Assert.Equal("1402/01/02", date.ToShamsi(1));
    }

    [Theory]
    [InlineData(13, 5, 9, "13:05:09")]
    [InlineData(0, 0, 0, "00:00:00")]
    public void ToClock(int hour, int minute, int second, string expected)
    {
        var date = new DateTime(2023, 3, 21, hour, minute, second);
        Assert.Equal(expected, date.ToClock());
    }

    [Theory]
    [InlineData("1402/01/01", true)]
    [InlineData("1402/13/01", false)]
    [InlineData("1402/01/32", false)]
    [InlineData("bad-format", false)]
    [InlineData("1299/01/01", false)]
    public void IsValidShamsiDate(string input, bool expected)
    {
        Assert.Equal(expected, input.IsValidShamsiDate());
    }

    [Theory]
    [InlineData("1402/01/01", 2023, 3, 21)]
    [InlineData("1402/01/02", 2023, 3, 22)]
    public void ToGregorian(string shamsiDate, int expectedYear, int expectedMonth, int expectedDay)
    {
        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay), shamsiDate.ToGregorian());
    }

    [Fact]
    public void ToGregorian_ThrowsAlertException_WhenYearOutOfSupportedRange()
    {
        Assert.Throws<AlertException>(() => "9999/01/01".ToGregorian());
    }

    // 2023-03-20 دوشنبه، 2023-03-21 سه‌شنبه، ... 2023-03-23 پنج‌شنبه، 2023-03-24 جمعه، 2023-03-25 شنبه
    [Theory]
    [InlineData(2023, 3, 21, true, false)]  // سه‌شنبه، حتی با thursdayIsWeekend=true تعطیل نیست
    [InlineData(2023, 3, 23, true, true)]   // پنج‌شنبه، وقتی پنج‌شنبه هم تعطیل حساب بشه
    [InlineData(2023, 3, 23, false, false)] // پنج‌شنبه، وقتی فقط جمعه تعطیل حساب بشه
    [InlineData(2023, 3, 24, false, true)]  // جمعه، همیشه تعطیل
    [InlineData(2023, 3, 25, true, false)]  // شنبه، روز کاری
    public void IsWeekend(int year, int month, int day, bool thursdayIsWeekend, bool expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).IsWeekend(thursdayIsWeekend));
    }

    [Fact]
    public void NextWorkingDay_SkipsThursdayAndFriday()
    {
        // چهارشنبه 2023-03-22 -> پنج‌شنبه و جمعه تعطیل -> شنبه 2023-03-25
        var result = new DateTime(2023, 3, 22).NextWorkingDay();
        Assert.Equal(new DateTime(2023, 3, 25), result);
    }

    [Fact]
    public void AddWorkingDays_SkipsWeekendsWhileCounting()
    {
        // دوشنبه 2023-03-20 + ۵ روز کاری (با عبور از پنج‌شنبه/جمعه) -> دوشنبه 2023-03-27
        var result = new DateTime(2023, 3, 20).AddWorkingDays(5);
        Assert.Equal(new DateTime(2023, 3, 27), result);
    }

    [Fact]
    public void AddWorkingDays_ReturnsOriginalDate_WhenZeroDays()
    {
        var date = new DateTime(2023, 3, 20);
        Assert.Equal(date, date.AddWorkingDays(0));
    }

    [Fact]
    public void AddWorkingDays_Throws_WhenNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DateTime.Today.AddWorkingDays(-1));
    }
}
