using System;
using System.Collections.Generic;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای <see cref="IReadOnlyDictionary{TKey, TValue}"/>.</summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// در صورت وجود کلید مقدارش را برمی‌گرداند، وگرنه <paramref name="defaultValue"/> را؛
        /// جایگزین الگوی <c>dict.TryGetValue(key, out var v) ? v : defaultValue</c>.
        /// عمداً هم‌نام با <c>CollectionExtensions.GetValueOrDefault</c> (.NET Core 2.0+) نیست تا در پروژه‌های
        /// net6+ با <c>using System.Collections.Generic</c> همزمان، خطای Ambiguous call رخ ندهد.
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default!)
        {
            if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
