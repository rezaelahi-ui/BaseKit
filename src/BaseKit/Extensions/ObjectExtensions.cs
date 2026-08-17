using System;
using System.Diagnostics.CodeAnalysis;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension عمومی روی هر نوع (بررسی null/empty رشته، مقایسه، fluent side-effect).</summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// بررسی می‌کند رشته null، خالی یا فقط whitespace است.
        /// در صورت false بودن نتیجه، کامپایلر <paramref name="str"/> را non-null در نظر می‌گیرد.
        /// </summary>
        public static bool IsEmpty([NotNullWhen(false)] this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// بررسی می‌کند رشته null، خالی یا فقط whitespace نیست.
        /// در صورت true بودن نتیجه، کامپایلر <paramref name="str"/> را non-null در نظر می‌گیرد.
        /// </summary>
        public static bool IsNotEmpty([NotNullWhen(true)] this string? str)
        {
            return !str.IsEmpty();
        }

        /// <summary>
        /// بررسی می‌کند مقدار برابر یکی از <paramref name="values"/> است؛ جایگزین زنجیره‌ی <c>==</c>/<c>||</c>
        /// (مثل <c>status.In(Status.Active, Status.Pending)</c> به‌جای <c>status == Status.Active || status == Status.Pending</c>).
        /// </summary>
        public static bool In<T>(this T value, params T[] values)
        {
            if (values is null) throw new ArgumentNullException(nameof(values));

            return Array.IndexOf(values, value) >= 0;
        }

        /// <summary>
        /// اجرای یک side-effect (مثل لاگ کردن) روی مقدار و برگرداندن خودِ همان مقدار؛ برای وسط یک زنجیره‌ی فلوئنت
        /// بدون شکستنش (مثل <c>GetData().Tap(x => logger.Log(x)).Process()</c>).
        /// </summary>
        public static T Tap<T>(this T obj, Action<T> action)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (action is null) throw new ArgumentNullException(nameof(action));

            action(obj);
            return obj;
        }
    }
}
