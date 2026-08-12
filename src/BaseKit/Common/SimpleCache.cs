using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace BaseKit.Common
{
    /// <summary>
    /// کش ساده‌ی in-memory با پشتیبانی از expiration؛ برای پروژه‌های کوچکی که نیازی به Redis/MemoryCache ندارند.
    /// Thread-safe (مبتنی بر ConcurrentDictionary).
    /// </summary>
    public class SimpleCache<TKey, TValue> where TKey : notnull
    {
        private sealed class Entry
        {
            public Entry(TValue value, DateTime? expiresAt)
            {
                Value = value;
                ExpiresAt = expiresAt;
            }

            public TValue Value { get; }
            public DateTime? ExpiresAt { get; }
            public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
        }

        private readonly ConcurrentDictionary<TKey, Entry> _store = new();

        public int Count => _store.Count;

        /// <summary>ذخیره‌ی یک مقدار؛ اگر <paramref name="expiration"/> داده نشود، مقدار منقضی نمی‌شود.</summary>
        public void Set(TKey key, TValue value, TimeSpan? expiration = null)
        {
            var expiresAt = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : (DateTime?)null;
            _store[key] = new Entry(value, expiresAt);
        }

        /// <summary>تلاش برای خواندن مقدار؛ اگر منقضی یا موجود نباشد false برمی‌گرداند (و در صورت منقضی بودن آن را حذف می‌کند).</summary>
        public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (_store.TryGetValue(key, out var entry) && !entry.IsExpired)
            {
                value = entry.Value;
                return true;
            }

            _store.TryRemove(key, out _);
            value = default;
            return false;
        }

        /// <summary>اگر مقدار موجود و معتبر باشد آن را برمی‌گرداند، وگرنه با <paramref name="factory"/> می‌سازد و ذخیره می‌کند.</summary>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory, TimeSpan? expiration = null)
        {
            if (TryGet(key, out var existing))
                return existing;

            var value = factory(key);
            Set(key, value, expiration);
            return value;
        }

        public void Remove(TKey key) => _store.TryRemove(key, out _);

        public void Clear() => _store.Clear();
    }
}
