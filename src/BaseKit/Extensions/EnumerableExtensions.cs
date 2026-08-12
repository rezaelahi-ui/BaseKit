using System;
using System.Collections.Generic;
using System.Linq;
using BaseKit.Common;

namespace BaseKit.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>اجرای یک Action روی تک‌تک آیتم‌های دنباله (LINQ همچین متدی ندارد).</summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (action is null) throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// تقسیم دنباله به دسته‌های حداکثر <paramref name="size"/> تایی.
        /// عمداً هم‌نام با <c>Enumerable.Chunk</c> (.NET 6+) نیست تا در پروژه‌های net6+ با
        /// <c>using System.Linq</c> همزمان، خطای Ambiguous call رخ ندهد.
        /// </summary>
        public static IEnumerable<List<T>> ChunkBy<T>(this IEnumerable<T> source, int size)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), "اندازه بايد حداقل ۱ باشد");

            return ChunkByIterator(source, size);
        }

        private static IEnumerable<List<T>> ChunkByIterator<T>(IEnumerable<T> source, int size)
        {
            var chunk = new List<T>(size);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == size)
                {
                    yield return chunk;
                    chunk = new List<T>(size);
                }
            }

            if (chunk.Count > 0)
                yield return chunk;
        }

        /// <summary>
        /// حذف آیتم‌های تکراری بر اساس یک کلید انتخابی، اولین رخداد هر کلید حفظ می‌شود.
        /// عمداً هم‌نام با <c>Enumerable.DistinctBy</c> (.NET 6+) نیست، به همان دلیل <see cref="ChunkBy{T}"/>.
        /// </summary>
        public static IEnumerable<T> DistinctByKey<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            return DistinctByKeyIterator(source, keySelector);
        }

        private static IEnumerable<T> DistinctByKeyIterator<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (seenKeys.Add(keySelector(item)))
                    yield return item;
            }
        }

        /// <summary>صفحه‌بندی یک دنباله؛ <paramref name="pageNumber"/> از ۱ شروع می‌شود.</summary>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "شماره صفحه بايد حداقل ۱ باشد");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "اندازه صفحه بايد حداقل ۱ باشد");

            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// یک صفحه از دنباله را به‌همراه اطلاعات کامل صفحه‌بندی (تعداد کل، تعداد صفحات، وجود صفحه بعدی/قبلی)
        /// در قالب <see cref="PagedResult{T}"/> برمی‌گرداند. دنباله برای شمارش تعداد کل، materialize می‌شود.
        /// </summary>
        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var materialized = source as IReadOnlyCollection<T> ?? source.ToList();
            var items = materialized.Page(pageNumber, pageSize).ToList();

            return new PagedResult<T>(items, pageNumber, pageSize, materialized.Count);
        }
    }
}
