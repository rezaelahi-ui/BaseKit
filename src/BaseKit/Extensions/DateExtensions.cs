using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using BaseKit.Exceptions;

namespace BaseKit.Extensions
{
    public static class DateExtensions
    {
        public static bool IsGreaterThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) > 0;
        }

        public static bool IsGreaterOrEqualsThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) >= 0;
        }

        public static bool IsLowerThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) < 0;
        }

        public static bool IsLowerOrEqualsThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) <= 0;
        }

        public static bool IsEqualThan(this string date, string targetDate)
        {
            return string.CompareOrdinal(date, targetDate) == 0;
        }

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

        public static string GetSafeValue(this string st, object? defaultValue = null)
        {
            if (st.IsEmpty())
                return defaultValue?.ToString() ?? "";
            return st;
        }

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
    }
}
