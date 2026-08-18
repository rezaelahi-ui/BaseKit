using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BaseKit.Exceptions;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای پارس/تبدیل و پردازش رشته.</summary>
    public static class StringExtensions
    {
        /// <summary>تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد صحیح.</summary>
        public static int ToInt(this string str)
        {
            if (str.IsEmpty()) throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");
            str = str.Replace(",", "");
            var resultParse = int.TryParse(str, out var result);
            if (!resultParse)
                throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");
            return result;
        }

        /// <summary>
        /// تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد صحیح؛ در صورت خالی/نامعتبر بودن به‌جای throw کردن null برمی‌گرداند.
        /// </summary>
        public static int? ToIntOrNull(this string? str)
        {
            if (str.IsEmpty()) return null;
            return int.TryParse(str.Replace(",", ""), out var result) ? result : null;
        }

        /// <summary>
        /// تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد اعشاری decimal؛ در صورت خالی/نامعتبر بودن به‌جای throw کردن null برمی‌گرداند.
        /// </summary>
        public static decimal? ToDecimalOrNull(this string? str)
        {
            if (str.IsEmpty()) return null;
            return decimal.TryParse(str.Replace(",", ""), out var result) ? result : null;
        }

        /// <summary>
        /// تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد اعشاری double؛ در صورت خالی/نامعتبر بودن به‌جای throw کردن null برمی‌گرداند.
        /// </summary>
        public static double? ToDoubleOrNull(this string? str)
        {
            if (str.IsEmpty()) return null;
            return double.TryParse(str.Replace(",", ""), out var result) ? result : null;
        }

        /// <summary>
        /// تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد صحیح بزرگ long؛ در صورت خالی/نامعتبر بودن به‌جای throw کردن null برمی‌گرداند.
        /// </summary>
        public static long? ToLongOrNull(this string? str)
        {
            if (str.IsEmpty()) return null;
            return long.TryParse(str.Replace(",", ""), out var result) ? result : null;
        }

        /// <summary>تبدیل رشته به <see cref="Uri"/>؛ باید با http/https شروع شود.</summary>
        public static Uri ToUri(this string str)
        {
            if (str.IsEmpty())
                throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");

            if (!str.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                throw new AlertException("آدرس سرويس بايد با HTTP و يا HTTPS شروع شود");

            if (str.EndsWith("/"))
            {
                str = str.TrimEnd('/');
            }

            if (!Uri.TryCreate(str, UriKind.Absolute, out var result))
                throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");

            return result;
        }

        /// <summary>تبدیل رشته به <see cref="IPAddress"/> (فرمت IPv4، چهار بخش نقطه‌جدا).</summary>
        public static IPAddress ToIp(this string str)
        {
            if (str.IsEmpty())
                throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");

            var parts = str.Split('.');
            if (parts.Length != 4 || parts.Any(p => !byte.TryParse(p, out _)))
                throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");

            return IPAddress.Parse(str);
        }

        /// <summary>تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد اعشاری decimal.</summary>
        public static decimal ToDecimal(this string str)
        {
            if (str.IsEmpty()) throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");
            str = str.Replace(",", "");
            var resultParse = decimal.TryParse(str, out var result);
            if (!resultParse)
                throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");
            return result;
        }

        /// <summary>تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد اعشاری double.</summary>
        public static double ToDouble(this string str)
        {
            if (str.IsEmpty()) throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");
            str = str.Replace(",", "");
            var resultParse = double.TryParse(str, out var result);
            if (!resultParse)
                throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");
            return result;
        }

        /// <summary>تبدیل رشته (با پشتیبانی از جداکننده‌ی کاما) به عدد صحیح بزرگ long.</summary>
        public static long ToLong(this string str)
        {
            if (str.IsEmpty()) throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");
            str = str.Replace(",", "");
            var resultParse = long.TryParse(str, out var result);
            if (!resultParse)
                throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");
            return result;
        }

        /// <summary>
        /// تبديل رشته به bool. مقادير پذيرفته‌شده: 1/0، true/false، yes/no، بله/خير.
        /// </summary>
        public static bool ToBool(this string str)
        {
            if (str.IsEmpty()) throw new ArgumentNullException(nameof(str), "مقدار وارد شده خالي يا null است");
            var value = str.Trim();

            if (bool.TryParse(value, out var boolResult))
                return boolResult;

            if (value == "1" || value.Equals("yes", StringComparison.OrdinalIgnoreCase) || value == "بله")
                return true;

            if (value == "0" || value.Equals("no", StringComparison.OrdinalIgnoreCase) || value == "خير" || value == "خیر")
                return false;

            throw new FormatException($"فرمت وارد شده {str} قابل تبديل نيست");
        }

        private static readonly char[] PersianDigits = { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };

        /// <summary>تبديل ارقام انگليسي داخل رشته به ارقام فارسي.</summary>
        public static string ToPersianDigits(this string str)
        {
            if (str.IsEmpty()) return str;

            var chars = str.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= '0' && chars[i] <= '9')
                    chars[i] = PersianDigits[chars[i] - '0'];
            }

            return new string(chars);
        }

        /// <summary>تبديل ارقام فارسي/عربي داخل رشته به ارقام انگليسي.</summary>
        public static string ToEnglishDigits(this string str)
        {
            if (str.IsEmpty()) return str;

            var chars = str.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                var c = chars[i];
                if (c >= '۰' && c <= '۹')
                    chars[i] = (char)('0' + (c - '۰'));
                else if (c >= '٠' && c <= '٩')
                    chars[i] = (char)('0' + (c - '٠'));
            }

            return new string(chars);
        }

        /// <summary>تبديل حروف عربي رايج (ي، ك) به معادل فارسي‌شان (ی، ک).</summary>
        public static string NormalizeArabicChars(this string str)
        {
            if (str.IsEmpty()) return str;

            return str.Replace('ي', 'ی').Replace('ك', 'ک');
        }

        /// <summary>
        /// نمايش بخشي از رشته با ماسک؛ مثلاً براي شماره موبايل يا کارت بانکي.
        /// اگر طول رشته کمتر از مجموع بخش‌هاي نمايان باشد، کل رشته ماسک مي‌شود.
        /// </summary>
        public static string Mask(this string str, int visibleStart = 4, int visibleEnd = 4, char maskChar = '*', int maskLength = 3)
        {
            if (str.IsEmpty()) return str;

            if (str.Length <= visibleStart + visibleEnd)
                return new string(maskChar, str.Length);

            var start = str.Substring(0, visibleStart);
            var end = str.Substring(str.Length - visibleEnd);
            return start + new string(maskChar, maskLength) + end;
        }

        /// <summary>
        /// کوتاه کردن متن تا حداکثر طول مشخص، با حفظ کلمه کامل (بدون بريدن وسط يک کلمه) و افزودن پسوند.
        /// </summary>
        public static string Truncate(this string str, int maxLength, string suffix = "...")
        {
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), "طول وارد شده نمي‌تواند منفي باشد");

            if (str.IsEmpty()) return str;
            if (str.Length <= maxLength) return str;

            var truncated = str.Substring(0, maxLength);
            var lastSpace = truncated.LastIndexOf(' ');
            if (lastSpace > 0)
                truncated = truncated.Substring(0, lastSpace);

            return truncated + suffix;
        }

        // نگاشت کلید-به-کلید بین حروف کیبورد استاندارد فارسی (ISIRI 9147) و کیبورد انگلیسی (QWERTY)،
        // برای اصلاح متنی که با چیدمان اشتباه کیبورد تایپ شده (مثلاً نیت فارسی ولی روی کیبورد انگلیسی).
        // فقط حروف/چند نشانه‌ی پرکاربرد پوشش داده می‌شود؛ ارقام و بقیه‌ی نشانه‌ها بدون تغییر باقی می‌مانند.
        private static readonly Dictionary<char, char> EnglishToPersianKeyMap = new()
        {
            ['q'] = 'ض', ['w'] = 'ص', ['e'] = 'ث', ['r'] = 'ق', ['t'] = 'ف', ['y'] = 'غ',
            ['u'] = 'ع', ['i'] = 'ه', ['o'] = 'خ', ['p'] = 'ح', ['['] = 'ج', [']'] = 'چ',
            ['a'] = 'ش', ['s'] = 'س', ['d'] = 'ی', ['f'] = 'ب', ['g'] = 'ل', ['h'] = 'ا',
            ['j'] = 'ت', ['k'] = 'ن', ['l'] = 'م', [';'] = 'ک', ['\''] = 'گ',
            ['z'] = 'ظ', ['x'] = 'ط', ['c'] = 'ز', ['v'] = 'ر', ['b'] = 'ذ', ['n'] = 'د',
            ['m'] = 'ئ', [','] = 'و', ['.'] = '،', ['/'] = '.',
        };

        private static readonly Dictionary<char, char> PersianToEnglishKeyMap =
            EnglishToPersianKeyMap.ToDictionary(kv => kv.Value, kv => kv.Key);

        /// <summary>
        /// تبدیل متنی که با نیت فارسی ولی روی کیبورد با چیدمان انگلیسی تایپ شده (مثل «hpd» به‌جای «الف»)
        /// به معادل فارسی‌اش، بر اساس موقعیت کلیدها در چیدمان استاندارد فارسی (ISIRI 9147).
        /// ارقام و نشانه‌های پوشش‌داده‌نشده بدون تغییر باقی می‌مانند.
        /// </summary>
        public static string ToPersianKeyboard(this string str)
        {
            if (str.IsEmpty()) return str;

            var builder = new StringBuilder(str.Length);
            foreach (var c in str)
            {
                var lower = char.ToLowerInvariant(c);
                builder.Append(EnglishToPersianKeyMap.TryGetValue(lower, out var persianChar) ? persianChar : c);
            }

            return builder.ToString();
        }

        /// <summary>
        /// عکس <see cref="ToPersianKeyboard"/>: تبدیل متنی که با نیت انگلیسی ولی روی کیبورد فارسی تایپ شده
        /// به معادل انگلیسی‌اش (حروف کوچک).
        /// </summary>
        public static string ToEnglishKeyboard(this string str)
        {
            if (str.IsEmpty()) return str;

            var builder = new StringBuilder(str.Length);
            foreach (var c in str)
                builder.Append(PersianToEnglishKeyMap.TryGetValue(c, out var englishChar) ? englishChar : c);

            return builder.ToString();
        }
    }
}
