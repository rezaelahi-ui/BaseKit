using System;
using System.Diagnostics;
using System.Text.Json;

namespace BaseKit.Extensions
{
    /// <summary>ابزارهای کمکی برای دیباگ/لاگ سریع، مبتنی بر <see cref="JsonSerializer"/>.</summary>
    public static class DebugExtensions
    {
        private static readonly JsonSerializerOptions IndentedOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        /// <summary>سریالایز خوانا (JSON indented) از یک آبجکت؛ مناسب پرینت سریع برای دیباگ (شبیه LINQPad.Dump).</summary>
        public static string Dump(this object? source)
            => source is null ? "null" : JsonSerializer.Serialize(source, source.GetType(), IndentedOptions);

        /// <summary>سریالایز فشرده به JSON.</summary>
        public static string ToJson<T>(this T source) => JsonSerializer.Serialize(source);

        /// <summary>دیسریالایز یک رشته‌ی JSON به نوع مشخص‌شده.</summary>
        public static T? FromJson<T>(this string json)
        {
            if (json.IsEmpty()) throw new ArgumentNullException(nameof(json), "مقدار وارد شده خالي يا null است");
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>اجرای یک <see cref="Action"/> و اندازه‌گیری زمان اجرای آن؛ برای پروفایلینگ سریع بدون <see cref="Stopwatch"/> دستی.</summary>
        public static TimeSpan Measure(this Action action)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));

            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>اجرای یک <see cref="Func{T}"/>، برگرداندن نتیجه به‌همراه زمان اجرا.</summary>
        public static (T Result, TimeSpan Elapsed) Measure<T>(this Func<T> func)
        {
            if (func is null) throw new ArgumentNullException(nameof(func));

            var stopwatch = Stopwatch.StartNew();
            var result = func();
            stopwatch.Stop();
            return (result, stopwatch.Elapsed);
        }
    }
}
