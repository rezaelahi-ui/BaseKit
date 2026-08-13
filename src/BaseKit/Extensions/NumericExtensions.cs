using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BaseKit.Common;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای فرمت‌بندی و تبدیل انواع عددی.</summary>
    public static class NumericExtensions
    {
        /// <summary>ساخت یک <see cref="Money"/> از مبلغ و واحد پول.</summary>
        public static Money ToMoney(this decimal amount, string currency) => new(amount, currency);

        /// <summary>فرمت عدد صحیح با جداکننده‌ی هزارگان (مثل 1,234,567).</summary>
        public static string ToSeparatedString(this int value)
            => value.ToString("#,0", CultureInfo.InvariantCulture);

        /// <summary>فرمت عدد صحیح بزرگ با جداکننده‌ی هزارگان.</summary>
        public static string ToSeparatedString(this long value)
            => value.ToString("#,0", CultureInfo.InvariantCulture);

        /// <summary>فرمت عدد اعشاری با جداکننده‌ی هزارگان (حداکثر دو رقم اعشار).</summary>
        public static string ToSeparatedString(this decimal value)
            => value.ToString("#,0.##", CultureInfo.InvariantCulture);

        /// <summary>فرمت عدد اعشاری با جداکننده‌ی هزارگان (حداکثر دو رقم اعشار).</summary>
        public static string ToSeparatedString(this double value)
            => value.ToString("#,0.##", CultureInfo.InvariantCulture);

        /// <summary>
        /// فرمت عدد به‌صورت جداشده با کاما و ارقام فارسی، به‌همراه واحد پولی (پیش‌فرض «ریال»).
        /// </summary>
        public static string ToPersianCurrency(this long value, string unit = "ریال")
        {
            var formatted = value.ToSeparatedString().ToPersianDigits();
            return unit.IsEmpty() ? formatted : $"{formatted} {unit}";
        }

        /// <summary>
        /// فرمت عدد به‌صورت جداشده با کاما و ارقام فارسی، به‌همراه واحد پولی (پیش‌فرض «ریال»).
        /// </summary>
        public static string ToPersianCurrency(this decimal value, string unit = "ریال")
        {
            var formatted = value.ToSeparatedString().ToPersianDigits();
            return unit.IsEmpty() ? formatted : $"{formatted} {unit}";
        }

        private static readonly string[] Ones =
            { "", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };

        private static readonly string[] Teens =
            { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };

        private static readonly string[] Tens =
            { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };

        private static readonly string[] Hundreds =
            { "", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };

        private static readonly string[] Scales = { "", "هزار", "میلیون", "میلیارد", "تریلیون" };

        /// <summary>
        /// تبدیل عدد به حروف فارسی (برای چک/فاکتور). حداکثر مقدار پشتیبانی‌شده حدود ۹۹۹ تریلیون است.
        /// </summary>
        public static string ToPersianWords(this long number)
        {
            if (number == 0) return "صفر";

            var isNegative = number < 0;
            var absolute = isNegative ? (ulong)(-number) : (ulong)number;

            var groups = new List<int>();
            while (absolute > 0)
            {
                groups.Add((int)(absolute % 1000));
                absolute /= 1000;
            }

            if (groups.Count > Scales.Length)
                throw new ArgumentOutOfRangeException(nameof(number), "عدد وارد شده بزرگ‌تر از محدوده پشتیبانی‌شده است");

            var parts = new List<string>();
            for (var i = groups.Count - 1; i >= 0; i--)
            {
                if (groups[i] == 0) continue;

                var words = ConvertThreeDigits(groups[i]);
                if (i > 0)
                    words += " " + Scales[i];

                parts.Add(words);
            }

            var result = string.Join(" و ", parts);
            return isNegative ? "منفی " + result : result;
        }

        private static string ConvertThreeDigits(int number)
        {
            var parts = new List<string>();

            var hundred = number / 100;
            var remainder = number % 100;

            if (hundred > 0) parts.Add(Hundreds[hundred]);

            if (remainder >= 10 && remainder < 20)
            {
                parts.Add(Teens[remainder - 10]);
            }
            else
            {
                var ten = remainder / 10;
                var one = remainder % 10;
                if (ten > 0) parts.Add(Tens[ten]);
                if (one > 0) parts.Add(Ones[one]);
            }

            return string.Join(" و ", parts);
        }
    }
}
