using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension مبتنی بر Reflection/JSON برای clone کردن و بررسی آبجکت‌ها.</summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// یک کپی مستقل (deep clone) از آبجکت می‌سازد، با سریالایز/دیسریالایز JSON.
        /// برای آبجکت‌های ساده (POCO/DTO) مناسب است؛ به گراف‌های چرخه‌دار توصیه نمی‌شود.
        /// </summary>
        public static T Clone<T>(this T source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(json)!;
        }

        /// <summary>
        /// خواص عمومی instance آبجکت را به یک Dictionary تبدیل می‌کند (مناسب برای لاگ/دیباگ).
        /// </summary>
        public static Dictionary<string, object?> ToDictionary(this object source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return source.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetIndexParameters().Length == 0 && p.CanRead)
                .ToDictionary(p => p.Name, object? (p) => p.GetValue(source));
        }
    }
}
