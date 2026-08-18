using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای کار با <see cref="IList"/>/<see cref="IEnumerable"/> و <see cref="List{T}"/>.</summary>
    public static class ListExtensions
    {
        /// <summary>بررسی می‌کند لیست null یا بدون عضو است.</summary>
        public static bool IsEmpty([NotNullWhen(false)] this IList? st)
            => st is null || st.Count == 0;

        /// <summary>بررسی می‌کند دنباله null یا بدون عضو است.</summary>
        public static bool IsEmpty([NotNullWhen(false)] this IEnumerable? enumerable)
            => enumerable is null || !enumerable.Cast<object>().Any();

        /// <summary>بررسی می‌کند دنباله null یا بدون عضو نیست.</summary>
        public static bool IsNotEmpty([NotNullWhen(true)] this IEnumerable? st)
            => !st.IsEmpty();

        /// <summary>ساخت رشته‌ای جداشده با کاما از مقدار انتخاب‌شده‌ی هر عضو لیست.</summary>
        public static string GetJoinedNames<T>(this List<T> st, Expression<Func<T, object>> selector)
        {
            if (st.IsEmpty()) return string.Empty;
            return string.Join(",", st.Select(selector.Compile()));
        }

        /// <summary>
        /// بررسی می‌کند دو لیست از نظر تعداد یا محتوا (بدون توجه به ترتیب) با هم تفاوت دارند.
        /// </summary>
        public static bool HasChanges<T>(this List<T> oldList, List<T> newList, IEqualityComparer<T>? comparer = null)
        {
            if (oldList.Count != newList.Count)
                return true;

            var comparerToUse = comparer ?? EqualityComparer<T>.Default;

            var oldSet = new HashSet<T>(oldList, comparerToUse);
            var newSet = new HashSet<T>(newList, comparerToUse);

            return !oldSet.SetEquals(newSet);
        }

        /// <summary>
        /// دسترسی امن به ایندکس؛ اگر خارج از محدوده باشد <paramref name="defaultValue"/> را برمی‌گرداند
        /// به‌جای پرتاب <see cref="ArgumentOutOfRangeException"/>. برخلاف <c>Enumerable.ElementAtOrDefault</c>
        /// (که فقط <c>default(T)</c> می‌دهد)، اجازه‌ی مقدار پیش‌فرض دلخواه را هم می‌دهد.
        /// </summary>
        public static T GetOrDefault<T>(this IList<T> list, int index, T defaultValue = default!)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));

            return index >= 0 && index < list.Count ? list[index] : defaultValue;
        }
    }
}
