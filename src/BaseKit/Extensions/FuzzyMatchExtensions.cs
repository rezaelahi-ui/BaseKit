using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseKit.Extensions
{
    /// <summary>
    /// متدهای extension برای محاسبه‌ی شباهت متنی (fuzzy matching) بر پایه‌ی فاصله‌ی ویرایشی Levenshtein.
    /// مناسب جستجوی فازی، تشخیص رکوردهای تکراری با غلط املایی، و auto-complete هوشمند.
    /// </summary>
    public static class FuzzyMatchExtensions
    {
        /// <summary>
        /// تعداد کمینه‌ی عملیات ویرایشی (درج/حذف/جایگزینی یک کاراکتر) لازم برای تبدیل <paramref name="source"/> به <paramref name="target"/>.
        /// </summary>
        public static int LevenshteinDistance(this string source, string target)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (target is null) throw new ArgumentNullException(nameof(target));

            var sourceLength = source.Length;
            var targetLength = target.Length;

            if (sourceLength == 0) return targetLength;
            if (targetLength == 0) return sourceLength;

            var previousRow = new int[targetLength + 1];
            var currentRow = new int[targetLength + 1];

            for (var j = 0; j <= targetLength; j++)
                previousRow[j] = j;

            for (var i = 1; i <= sourceLength; i++)
            {
                currentRow[0] = i;

                for (var j = 1; j <= targetLength; j++)
                {
                    var cost = source[i - 1] == target[j - 1] ? 0 : 1;
                    currentRow[j] = Math.Min(
                        Math.Min(currentRow[j - 1] + 1, previousRow[j] + 1),
                        previousRow[j - 1] + cost);
                }

                (previousRow, currentRow) = (currentRow, previousRow);
            }

            return previousRow[targetLength];
        }

        /// <summary>درصد شباهت دو رشته، بین ۰ (کاملاً متفاوت) و ۱ (کاملاً یکسان)؛ بر پایه‌ی <see cref="LevenshteinDistance"/>.</summary>
        public static double SimilarityTo(this string source, string target)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (target is null) throw new ArgumentNullException(nameof(target));

            var maxLength = Math.Max(source.Length, target.Length);
            if (maxLength == 0) return 1.0;

            var distance = source.LevenshteinDistance(target);
            return 1.0 - (double)distance / maxLength;
        }

        /// <summary>بررسی می‌کند شباهت دو رشته حداقل به‌اندازه‌ی <paramref name="threshold"/> (پیش‌فرض ۰.۸ = ۸۰٪) است.</summary>
        public static bool IsSimilarTo(this string source, string target, double threshold = 0.8)
        {
            if (threshold < 0 || threshold > 1)
                throw new ArgumentOutOfRangeException(nameof(threshold), "آستانه بايد بين ۰ و ۱ باشد");

            return source.SimilarityTo(target) >= threshold;
        }

        /// <summary>نزدیک‌ترین آیتم لیست به <paramref name="query"/> (بیشترین شباهت)؛ اگر لیست خالی باشد null برمی‌گرداند.</summary>
        public static string? FindBestMatch(this IEnumerable<string> source, string query)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (query is null) throw new ArgumentNullException(nameof(query));

            string? best = null;
            var bestScore = -1.0;

            foreach (var item in source)
            {
                var score = item.SimilarityTo(query);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = item;
                }
            }

            return best;
        }

        /// <summary>
        /// همه‌ی آیتم‌های لیست که شباهتشان به <paramref name="query"/> حداقل <paramref name="threshold"/> (پیش‌فرض ۰.۸) است،
        /// مرتب‌شده بر اساس شباهت نزولی.
        /// </summary>
        public static IEnumerable<(string Item, double Score)> FindSimilar(
            this IEnumerable<string> source, string query, double threshold = 0.8)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (threshold < 0 || threshold > 1)
                throw new ArgumentOutOfRangeException(nameof(threshold), "آستانه بايد بين ۰ و ۱ باشد");

            return source
                .Select(item => (Item: item, Score: item.SimilarityTo(query)))
                .Where(x => x.Score >= threshold)
                .OrderByDescending(x => x.Score);
        }
    }
}
