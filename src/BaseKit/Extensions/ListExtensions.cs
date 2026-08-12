using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace BaseKit.Extensions
{
    public static class ListExtensions
    {
        public static bool IsEmpty([NotNullWhen(false)] this IList? st)
            => st is null || st.Count == 0;

        public static bool IsEmpty([NotNullWhen(false)] this IEnumerable? enumerable)
            => enumerable is null || !enumerable.Cast<object>().Any();

        public static bool IsNotEmpty([NotNullWhen(true)] this IEnumerable? st)
            => !st.IsEmpty();

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
    }
}
