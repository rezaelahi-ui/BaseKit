using System;
using System.Text.Json;

namespace BaseKit.Extensions
{
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
    }
}
