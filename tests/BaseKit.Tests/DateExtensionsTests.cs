using System;
using System.Globalization;
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

    [Fact]
    public void GetPersianMonthName_And_GetPersianSeason_ForFarvardin()
    {
        var date = new DateTime(2023, 3, 21); // 1402/01/01
        Assert.Equal("فروردین", date.GetPersianMonthName());
        Assert.Equal("بهار", date.GetPersianSeason());
    }

    [Fact]
    public void GetPersianMonthName_And_GetPersianSeason_ForTir()
    {
        var date = "1402/04/01".ToGregorian();
        Assert.Equal("تیر", date.GetPersianMonthName());
        Assert.Equal("تابستان", date.GetPersianSeason());
    }

    // 2023-03-19 یکشنبه تا 2023-03-25 شنبه (هم‌راستا با کامنت خط ۱۲۰)
    [Theory]
    [InlineData(2023, 3, 19, "یکشنبه")]
    [InlineData(2023, 3, 20, "دوشنبه")]
    [InlineData(2023, 3, 21, "سه‌شنبه")]
    [InlineData(2023, 3, 22, "چهارشنبه")]
    [InlineData(2023, 3, 23, "پنج‌شنبه")]
    [InlineData(2023, 3, 24, "جمعه")]
    [InlineData(2023, 3, 25, "شنبه")]
    public void GetPersianDayName(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).GetPersianDayName());
    }

    [Fact]
    public void GetShamsiYear_And_GetShamsiMonth()
    {
        var date = new DateTime(2023, 3, 21); // 1402/01/01
        Assert.Equal(1402, date.GetShamsiYear());
        Assert.Equal(1, date.GetShamsiMonth());
    }

    [Fact]
    public void IsIranianHoliday_True_ForNowruz()
    {
        Assert.True(new DateTime(2023, 3, 21).IsIranianHoliday()); // 1402/01/01
    }

    [Fact]
    public void IsIranianHoliday_True_ForFriday()
    {
        Assert.True(new DateTime(2023, 3, 24).IsIranianHoliday()); // جمعه
    }

    [Fact]
    public void IsIranianHoliday_False_ForOrdinaryWorkday()
    {
        Assert.False(new DateTime(2023, 3, 26).IsIranianHoliday()); // 1402/01/06، یکشنبه، عادی
    }

    [Fact]
    public void IsIranianHoliday_RespectsThursdayIsWeekendFlag()
    {
        var thursday = new DateTime(2023, 3, 30); // 1402/01/10، خارج از تعطیلات ثابت نوروز
        Assert.True(thursday.IsIranianHoliday(thursdayIsWeekend: true));
        Assert.False(thursday.IsIranianHoliday(thursdayIsWeekend: false));
    }

    [Fact]
    public void IsIranianHoliday_True_ForCustomExtraHoliday()
    {
        var date = new DateTime(2023, 3, 26); // 1402/01/06، عادی مگر با extra
        Assert.True(date.IsIranianHoliday(new[] { "1402/01/06" }, thursdayIsWeekend: false));
    }

    [Fact]
    public void GetPersianDateInfo_ReturnsExpectedValues_ForFarvardinFirst()
    {
        var date = new DateTime(2023, 3, 21); // 1402/01/01، سه‌شنبه
        var info = date.GetPersianDateInfo();

        Assert.Equal(1402, info.Year);
        Assert.Equal(1, info.Month);
        Assert.Equal(1, info.Day);
        Assert.Equal(DayOfWeek.Tuesday, info.DayOfWeek);
        Assert.Equal("سه‌شنبه", info.DayName);
        Assert.Equal("فروردین", info.MonthName);
        Assert.Equal("بهار", info.SeasonName);
        Assert.Equal(31, info.DaysInMonth);
        Assert.Equal("1402/01/01", info.ShamsiDate);
        Assert.Equal("1402/01/01", info.MonthStartShamsi);
        Assert.Equal("1402/01/31", info.MonthEndShamsi);
        Assert.Equal("1402/01/01", info.SeasonStartShamsi);
        Assert.Equal("1402/03/31", info.SeasonEndShamsi);
        Assert.Equal("1402/01/01", info.YearStartShamsi);
        Assert.Equal(date, info.MonthStartDate);
        Assert.Equal(date, info.SeasonStartDate);
        Assert.Equal(date, info.YearStartDate);
    }

    [Fact]
    public void GetWeeksOfShamsiMonth_FirstWeekIsClippedToMonthStart()
    {
        var weeks = 1402.GetWeeksOfShamsiMonth(1);

        var firstWeek = weeks[0];
        Assert.Equal(1, firstWeek.WeekNumber);
        Assert.Equal(new DateTime(2023, 3, 21), firstWeek.StartDate);
        Assert.Equal(new DateTime(2023, 3, 24), firstWeek.EndDate);
        Assert.Equal("1402/01/01", firstWeek.StartDateShamsi);
        Assert.Equal("1402/01/04", firstWeek.EndDateShamsi);

        var lastWeek = weeks[^1];
        Assert.Equal(new DateTime(2023, 4, 20), lastWeek.EndDate);
        Assert.Equal("1402/01/31", lastWeek.EndDateShamsi);
    }

    [Fact]
    public void GetWeeksOfShamsiMonth_Throws_WhenMonthOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 1402.GetWeeksOfShamsiMonth(13));
    }

    [Fact]
    public void GetMonthsOfShamsiSeason_ReturnsThreeMonths()
    {
        var months = 1402.GetMonthsOfShamsiSeason(1);

        Assert.Equal(3, months.Count);
        Assert.Equal(1, months[0].MonthNumber);
        Assert.Equal("فروردین", months[0].MonthName);
        Assert.Equal("1402/01/01", months[0].StartDateShamsi);
        Assert.Equal("1402/01/31", months[0].EndDateShamsi);
        Assert.Equal(3, months[2].MonthNumber);
        Assert.Equal("خرداد", months[2].MonthName);
        Assert.Equal("1402/03/31", months[2].EndDateShamsi);
    }

    [Fact]
    public void GetMonthsOfShamsiSeason_Throws_WhenSeasonStartMonthInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 1402.GetMonthsOfShamsiSeason(2));
    }

    [Fact]
    public void GetSeasonsOfShamsiYear_ReturnsFourSeasons()
    {
        var pc = new PersianCalendar();
        var esfandDays = pc.GetDaysInMonth(1402, 12);

        var seasons = 1402.GetSeasonsOfShamsiYear();

        Assert.Equal(4, seasons.Count);
        Assert.Equal(1, seasons[0].SeasonNumber);
        Assert.Equal("بهار", seasons[0].SeasonName);
        Assert.Equal("1402/01/01", seasons[0].StartDateShamsi);
        Assert.Equal("1402/03/31", seasons[0].EndDateShamsi);
        Assert.Equal(4, seasons[3].SeasonNumber);
        Assert.Equal("زمستان", seasons[3].SeasonName);
        Assert.Equal("1402/10/01", seasons[3].StartDateShamsi);
        Assert.Equal($"1402/12/{esfandDays:00}", seasons[3].EndDateShamsi);
    }

    [Fact]
    public void ToUnixTimestamp_CombinesShamsiDateAndTime()
    {
        var expected = new DateTimeOffset(new DateTime(2023, 3, 21, 13, 5, 9, DateTimeKind.Utc)).ToUnixTimeMilliseconds();
        Assert.Equal(expected, "1402/01/01".ToUnixTimestamp("13:05:09"));
    }

    [Fact]
    public void ToUnixTimestamp_DefaultsSecondsToZero_WhenOmitted()
    {
        var expected = new DateTimeOffset(new DateTime(2023, 3, 21, 13, 5, 0, DateTimeKind.Utc)).ToUnixTimeMilliseconds();
        Assert.Equal(expected, "1402/01/01".ToUnixTimestamp("13:05"));
    }

    [Theory]
    [InlineData("25:00")]
    [InlineData("13:60")]
    [InlineData("not-a-time")]
    public void ToUnixTimestamp_Throws_WhenTimeInvalid(string time)
    {
        Assert.Throws<FormatException>(() => "1402/01/01".ToUnixTimestamp(time));
    }
}
