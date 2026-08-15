using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using BaseKit.Exceptions;
using BaseKit.Models;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای کار با رشته‌های تاریخ (عمدتاً شمسی) و <see cref="DateTime"/>.</summary>
    public static class DateExtensions
    {
        /// <summary>مقایسه‌ی رشته‌ای دو تاریخ (فرمت YYYY/MM/DD)؛ true اگر <paramref name="date"/> بعد از <paramref name="targetDate"/> باشد.</summary>
        public static bool IsGreaterThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) > 0;
        }

        /// <summary>مقایسه‌ی رشته‌ای دو تاریخ؛ true اگر <paramref name="date"/> بعد از یا برابر <paramref name="targetDate"/> باشد.</summary>
        public static bool IsGreaterOrEqualsThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) >= 0;
        }

        /// <summary>مقایسه‌ی رشته‌ای دو تاریخ؛ true اگر <paramref name="date"/> قبل از <paramref name="targetDate"/> باشد.</summary>
        public static bool IsLowerThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) < 0;
        }

        /// <summary>مقایسه‌ی رشته‌ای دو تاریخ؛ true اگر <paramref name="date"/> قبل از یا برابر <paramref name="targetDate"/> باشد.</summary>
        public static bool IsLowerOrEqualsThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) <= 0;
        }

        /// <summary>مقایسه‌ی رشته‌ای دو تاریخ؛ true اگر برابر باشند.</summary>
        public static bool IsEqualThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) == 0;
        }

        /// <summary>محدود کردن تعداد بخش‌های جداشده با <paramref name="separator"/> به <paramref name="count"/> مورد؛ باقی با «...» جایگزین می‌شود.</summary>
        public static string Ellipsis(this string st, int count, string separator = ",")
        {
            if (st.IsEmpty())
                return st;

            var parts = st.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(p => p.Trim())
                          .Where(p => !string.IsNullOrEmpty(p))
                          .ToList();

            if (parts.Count <= count)
                return st;

            var visibleParts = parts.Take(count);
            return string.Join($"{separator} ", visibleParts) + $"{separator} ...";
        }

        /// <summary>اگر رشته خالی/null باشد <paramref name="defaultValue"/> (یا رشته‌ی خالی) برمی‌گرداند، وگرنه خود رشته را.</summary>
        public static string GetSafeValue(this string st, object? defaultValue = null)
        {
            if (st.IsEmpty())
                return defaultValue?.ToString() ?? "";
            return st;
        }

        /// <summary>تبدیل تاریخ میلادی به رشته‌ی تاریخ شمسی با فرمت YYYY/MM/DD.</summary>
        public static string ToShamsi(this DateTime date)
        {
            try
            {
                var pc = new PersianCalendar();
                return string.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("00"), pc.GetDayOfMonth(date).ToString("00"));
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>تبدیل تاریخ میلادی (پس از افزودن <paramref name="addDay"/> روز) به رشته‌ی تاریخ شمسی.</summary>
        public static string ToShamsi(this DateTime date, int addDay)
        {
            try
            {
                var pc = new PersianCalendar();
                date = date.AddDays(addDay);
                return string.Format("{0}/{1}/{2}", pc.GetYear(date), pc.GetMonth(date).ToString("00"), pc.GetDayOfMonth(date).ToString("00"));
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// بررسی می‌کند تاریخ در تعطیلات آخر هفته‌ی ایران (پنج‌شنبه و جمعه) قرار دارد.
        /// اگر فقط جمعه به‌عنوان تعطیل رسمی مدنظر است، پارامتر <paramref name="thursdayIsWeekend"/> را false بدهید.
        /// </summary>
        public static bool IsWeekend(this DateTime date, bool thursdayIsWeekend = true)
        {
            if (date.DayOfWeek == DayOfWeek.Friday) return true;
            return thursdayIsWeekend && date.DayOfWeek == DayOfWeek.Thursday;
        }

        /// <summary>نزدیک‌ترین روز کاری بعد از تاریخ داده‌شده (خود تاریخ ورودی را در نظر نمی‌گیرد).</summary>
        public static DateTime NextWorkingDay(this DateTime date, bool thursdayIsWeekend = true)
        {
            var next = date.AddDays(1);
            while (next.IsWeekend(thursdayIsWeekend))
                next = next.AddDays(1);

            return next;
        }

        /// <summary>افزودن N روز کاری (بدون احتساب تعطیلات آخر هفته) به تاریخ.</summary>
        public static DateTime AddWorkingDays(this DateTime date, int days, bool thursdayIsWeekend = true)
        {
            if (days < 0)
                throw new ArgumentOutOfRangeException(nameof(days), "تعداد روز نمي‌تواند منفي باشد");

            var result = date;
            for (var i = 0; i < days; i++)
                result = result.NextWorkingDay(thursdayIsWeekend);

            return result;
        }

        /// <summary>تبدیل بخش زمان تاریخ به رشته‌ی HH:mm:ss (با تقویم شمسی).</summary>
        public static string ToClock(this DateTime date)
        {
            try
            {
                var pc = new PersianCalendar();
                return string.Format("{0}:{1}:{2}", pc.GetHour(date).ToString("00"), pc.GetMinute(date).ToString("00"), pc.GetSecond(date).ToString("00"));
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// بررسی می‌کند رشته ورودی یک تاریخ شمسی معتبر با فرمت YYYY/MM/DD است.
        /// </summary>
        public static bool IsValidShamsiDate(this string date)
        {
            date = date.Trim();

            // بررسی الگوی 1400/01/01
            const string pattern = @"^\d{4}/\d{2}/\d{2}$";
            if (!Regex.IsMatch(date, pattern))
                return false;

            // جداسازی بخش‌ها
            var parts = date.Split('/');
            if (parts.Length != 3)
                return false;

            // تبدیل به عدد
            if (!int.TryParse(parts[0], out var year) ||
                !int.TryParse(parts[1], out var month) ||
                !int.TryParse(parts[2], out var day))
                return false;

            // اعتبارسنجی محدوده سال
            if (year < 1300 || year > 1500)
                return false;

            // اعتبارسنجی محدوده ماه
            if (month < 1 || month > 12)
                return false;

            // اعتبارسنجی محدوده روز
            if (day < 1 || day > 31)
                return false;

            // بررسی روزهای هر ماه شمسی: فروردین تا شهریور ۳۱ روزه
            if (month <= 6 && day > 31)
                return false;

            // مهر تا بهمن ۳۰ روزه
            if (month > 6 && month <= 11 && day > 30)
                return false;

            // اسفند حداکثر ۲۹ روز (به جز سال کبیسه)
            if (month == 12 && day > 29)
                return false;

            // در اسفندِ روز ۳۰، فقط سال‌های کبیسه معتبرند
            if (month == 12 && day == 30 && !IsLeapShamsiYear(year))
                return false;

            return true;
        }

        /// <summary>تشخیص سال کبیسه شمسی.</summary>
        private static bool IsLeapShamsiYear(int year)
        {
            var a = (year - 1342) % 33;
            var b = (year - 1343) % 33;
            var c = (year - 1344) % 33;

            return a == 0 || a == 1 || b == 0 || c == 0;
        }

        /// <summary>تبدیل رشته‌ی تاریخ شمسی (فرمت YYYY/MM/DD) به <see cref="DateTime"/> میلادی.</summary>
        public static DateTime ToGregorian(this string shamsiDate)
        {
            var pc = new PersianCalendar();
            try
            {
                if (!Regex.IsMatch(shamsiDate, @"^\d{4}/\d{2}/\d{2}$"))
                    throw new FormatException("فرمت بايد YYYY/MM/DD باشد");

                var year = int.Parse(shamsiDate.Substring(0, 4));
                var month = int.Parse(shamsiDate.Substring(5, 2));
                var day = int.Parse(shamsiDate.Substring(8, 2));
                return new DateTime(year, month, day, pc);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new AlertException("سال خارج از محدوده پشتيباني تقويم شمسي است");
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        private static readonly string[] PersianMonthNames =
        {
            "", "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
            "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
        };

        // اندیس‌ها با System.DayOfWeek هم‌راستا هستند (یکشنبه=۰ تا شنبه=۶).
        private static readonly string[] PersianDayNames =
        {
            "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"
        };

        private static readonly string[] PersianSeasonNames = { "بهار", "تابستان", "پاییز", "زمستان" };

        // تعطیلات رسمی ایران که تاریخ‌شان در تقویم شمسی ثابت است (مثل نوروز)؛ فرمت "MM/DD".
        // تعطیلات مذهبی/قمری (عاشورا، اعیاد و ...) چون سال به سال جابه‌جا می‌شوند اینجا نیستند و باید
        // از طریق پارامتر extraHolidaysShamsi در IsIranianHoliday به‌ازای هر سال جداگانه داده شوند.
        private static readonly HashSet<string> FixedShamsiHolidaysMonthDay = new()
        {
            "01/01", "01/02", "01/03", "01/04", // نوروز
            "01/12", // روز جمهوری اسلامی
            "01/13", // سیزده‌بدر
            "03/14", // رحلت امام خمینی
            "03/15", // قیام ۱۵ خرداد
            "11/22", // پیروزی انقلاب اسلامی
            "12/29", // روز ملی شدن صنعت نفت
        };

        /// <summary>نام فارسی ماه شمسیِ تاریخ (مثل «فروردین»).</summary>
        public static string GetPersianMonthName(this DateTime date)
        {
            var pc = new PersianCalendar();
            return PersianMonthNames[pc.GetMonth(date)];
        }

        /// <summary>نام فارسی روز هفته‌ی تاریخ (مثل «شنبه»)؛ در تقویم میلادی و شمسی یکسان است.</summary>
        public static string GetPersianDayName(this DateTime date) => PersianDayNames[(int)date.DayOfWeek];

        /// <summary>نام فارسی فصلِ تاریخ در تقویم شمسی (مثل «بهار»).</summary>
        public static string GetPersianSeason(this DateTime date)
        {
            var pc = new PersianCalendar();
            var month = pc.GetMonth(date);
            return PersianSeasonNames[(month - 1) / 3];
        }

        /// <summary>سال شمسی تاریخ، به‌صورت عدد (بدون نیاز به parse کردن خروجی رشته‌ای <see cref="ToShamsi(DateTime)"/>).</summary>
        public static int GetShamsiYear(this DateTime date) => new PersianCalendar().GetYear(date);

        /// <summary>ماه شمسی تاریخ (۱ تا ۱۲)، به‌صورت عدد.</summary>
        public static int GetShamsiMonth(this DateTime date) => new PersianCalendar().GetMonth(date);

        /// <summary>
        /// بررسی می‌کند تاریخ، تعطیل رسمی ایران است: آخر هفته (<see cref="IsWeekend"/>) یا یکی از
        /// تعطیلات ثابت شمسی (نوروز و مشابه). تعطیلات مذهبی/قمری هرساله جابه‌جا می‌شوند، پس باید
        /// از طریق <paramref name="extraHolidaysShamsi"/> (رشته‌های تاریخ شمسی YYYY/MM/DD) اضافه شوند.
        /// </summary>
        public static bool IsIranianHoliday(this DateTime date, IEnumerable<string>? extraHolidaysShamsi = null, bool thursdayIsWeekend = true)
        {
            if (date.IsWeekend(thursdayIsWeekend)) return true;

            var shamsi = date.ToShamsi();
            if (shamsi.Length == 10 && FixedShamsiHolidaysMonthDay.Contains(shamsi.Substring(5)))
                return true;

            if (extraHolidaysShamsi is null) return false;

            foreach (var extra in extraHolidaysShamsi)
            {
                if (extra.IsNotEmpty() && extra.Trim() == shamsi)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// اطلاعات کامل تاریخ در تقویم شمسی: سال/ماه/روز/نام‌ها، به‌همراه بازه‌ی شروع و پایان
        /// ماه، فصل و سالی که این تاریخ در آن قرار دارد.
        /// </summary>
        public static PersianDateInfo GetPersianDateInfo(this DateTime date)
        {
            var pc = new PersianCalendar();
            var year = pc.GetYear(date);
            var month = pc.GetMonth(date);
            var day = pc.GetDayOfMonth(date);

            var seasonIndex = (month - 1) / 3;
            var seasonStartMonth = seasonIndex * 3 + 1;
            var seasonEndMonth = seasonStartMonth + 2;

            var daysInMonth = pc.GetDaysInMonth(year, month);
            var seasonEndDaysInMonth = pc.GetDaysInMonth(year, seasonEndMonth);
            var yearEndDaysInMonth = pc.GetDaysInMonth(year, 12);

            return new PersianDateInfo
            {
                Year = year,
                Month = month,
                Day = day,
                DayOfWeek = date.DayOfWeek,
                DayName = PersianDayNames[(int)date.DayOfWeek],
                MonthName = PersianMonthNames[month],
                SeasonName = PersianSeasonNames[seasonIndex],
                DaysInMonth = daysInMonth,
                DaysInYear = pc.GetDaysInYear(year),
                ShamsiDate = $"{year:0000}/{month:00}/{day:00}",
                MonthStartShamsi = $"{year:0000}/{month:00}/01",
                MonthEndShamsi = $"{year:0000}/{month:00}/{daysInMonth:00}",
                SeasonStartShamsi = $"{year:0000}/{seasonStartMonth:00}/01",
                SeasonEndShamsi = $"{year:0000}/{seasonEndMonth:00}/{seasonEndDaysInMonth:00}",
                YearStartShamsi = $"{year:0000}/01/01",
                YearEndShamsi = $"{year:0000}/12/{yearEndDaysInMonth:00}",
                MonthStartDate = new DateTime(year, month, 1, pc),
                MonthEndDate = new DateTime(year, month, daysInMonth, pc),
                SeasonStartDate = new DateTime(year, seasonStartMonth, 1, pc),
                SeasonEndDate = new DateTime(year, seasonEndMonth, seasonEndDaysInMonth, pc),
                YearStartDate = new DateTime(year, 1, 1, pc),
                YearEndDate = new DateTime(year, 12, yearEndDaysInMonth, pc),
            };
        }

        /// <summary>
        /// تفکیک یک ماه شمسی به هفته‌ها (هر هفته از شنبه شروع می‌شود؛ هفته‌ی اول/آخر به بازه‌ی ماه محدود می‌شود).
        /// </summary>
        public static List<WeekInfo> GetWeeksOfShamsiMonth(this int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "ماه بايد بين ۱ تا ۱۲ باشد");

            var pc = new PersianCalendar();
            var daysInMonth = pc.GetDaysInMonth(year, month);
            var firstDay = new DateTime(year, month, 1, pc);
            var lastDay = new DateTime(year, month, daysInMonth, pc);

            var current = firstDay;
            while (current.DayOfWeek != DayOfWeek.Saturday)
                current = current.AddDays(-1);

            var weeks = new List<WeekInfo>();
            var weekNumber = 1;
            var now = DateTime.Now;

            while (current <= lastDay)
            {
                var effectiveStart = current < firstDay ? firstDay : current;
                var weekEnd = current.AddDays(6);
                var effectiveEnd = weekEnd > lastDay ? lastDay : weekEnd;

                weeks.Add(new WeekInfo
                {
                    WeekNumber = weekNumber,
                    StartDate = effectiveStart,
                    EndDate = effectiveEnd,
                    StartDateShamsi = effectiveStart.ToShamsi(),
                    EndDateShamsi = effectiveEnd.ToShamsi(),
                    IsComplete = effectiveEnd <= now,
                });

                weekNumber++;
                current = current.AddDays(7);
            }

            return weeks;
        }

        /// <summary>تفکیک یک فصل شمسی (که با <paramref name="seasonStartMonth"/> یکی از ۱، ۴، ۷ یا ۱۰ مشخص می‌شود) به سه ماه.</summary>
        public static List<MonthInfo> GetMonthsOfShamsiSeason(this int year, int seasonStartMonth)
        {
            if (seasonStartMonth != 1 && seasonStartMonth != 4 && seasonStartMonth != 7 && seasonStartMonth != 10)
                throw new ArgumentOutOfRangeException(nameof(seasonStartMonth), "ماه شروع فصل بايد يکي از ۱، ۴، ۷ يا ۱۰ باشد");

            var pc = new PersianCalendar();
            var months = new List<MonthInfo>();
            var now = DateTime.Now;

            for (var i = 0; i < 3; i++)
            {
                var currentMonth = seasonStartMonth + i;
                var currentYear = year;
                if (currentMonth > 12)
                {
                    currentMonth -= 12;
                    currentYear++;
                }

                var daysInMonth = pc.GetDaysInMonth(currentYear, currentMonth);
                var startDate = new DateTime(currentYear, currentMonth, 1, pc);
                var endDate = new DateTime(currentYear, currentMonth, daysInMonth, pc);

                months.Add(new MonthInfo
                {
                    Year = currentYear,
                    MonthNumber = currentMonth,
                    MonthName = PersianMonthNames[currentMonth],
                    StartDate = startDate,
                    EndDate = endDate,
                    StartDateShamsi = startDate.ToShamsi(),
                    EndDateShamsi = endDate.ToShamsi(),
                    IsComplete = endDate <= now,
                });
            }

            return months;
        }

        /// <summary>تفکیک یک سال شمسی به چهار فصل (بهار تا زمستان).</summary>
        public static List<SeasonInfo> GetSeasonsOfShamsiYear(this int year)
        {
            var pc = new PersianCalendar();
            var seasons = new List<SeasonInfo>();
            var seasonStartMonths = new[] { 1, 4, 7, 10 };
            var now = DateTime.Now;

            for (var i = 0; i < 4; i++)
            {
                var startMonth = seasonStartMonths[i];
                var endMonth = startMonth + 2;
                var endDaysInMonth = pc.GetDaysInMonth(year, endMonth);

                var startDate = new DateTime(year, startMonth, 1, pc);
                var endDate = new DateTime(year, endMonth, endDaysInMonth, pc);

                seasons.Add(new SeasonInfo
                {
                    SeasonNumber = i + 1,
                    SeasonName = PersianSeasonNames[i],
                    StartDate = startDate,
                    EndDate = endDate,
                    StartDateShamsi = startDate.ToShamsi(),
                    EndDateShamsi = endDate.ToShamsi(),
                    IsComplete = endDate <= now,
                });
            }

            return seasons;
        }

        /// <summary>
        /// ترکیب یک تاریخ شمسی (YYYY/MM/DD) و رشته‌ی ساعت ("HH:mm" یا "HH:mm:ss") به Unix timestamp
        /// (میلی‌ثانیه، UTC)؛ برای مواردی که تاریخ و ساعت جدا از هم ذخیره شده‌اند.
        /// </summary>
        public static long ToUnixTimestamp(this string shamsiDate, string time)
        {
            if (time.IsEmpty()) throw new ArgumentNullException(nameof(time), "مقدار وارد شده خالي يا null است");

            var timeParts = time.Trim().Split(':');
            if (timeParts.Length is < 2 or > 3 ||
                !int.TryParse(timeParts[0], out var hour) || hour is < 0 or > 23 ||
                !int.TryParse(timeParts[1], out var minute) || minute is < 0 or > 59)
                throw new FormatException($"فرمت ساعت وارد شده {time} قابل تبديل نيست");

            var second = 0;
            if (timeParts.Length == 3 && (!int.TryParse(timeParts[2], out second) || second is < 0 or > 59))
                throw new FormatException($"فرمت ساعت وارد شده {time} قابل تبديل نيست");

            var gregorianDate = shamsiDate.ToGregorian();
            var dateTime = new DateTime(gregorianDate.Year, gregorianDate.Month, gregorianDate.Day, hour, minute, second, DateTimeKind.Utc);
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }
    }
}
