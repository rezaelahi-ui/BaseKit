using System;
using System.Diagnostics.CodeAnalysis;
using BaseKit.Extensions;

namespace BaseKit.Guards
{
    public static class GuardClauseExtensions
    {
        /// <summary>اگر <paramref name="value"/> null باشد ArgumentNullException می‌دهد؛ در غیر این صورت خودش را برمی‌گرداند.</summary>
        public static T Null<T>(this IGuardClause guard, [NotNull] T? value, string paramName)
            where T : class
        {
            if (value is null)
                throw new ArgumentNullException(paramName, "مقدار وارد شده نمي‌تواند null باشد");

            return value;
        }

        /// <summary>اگر <paramref name="value"/> خالی/فقط-whitespace یا null باشد ArgumentException می‌دهد.</summary>
        public static string Empty(this IGuardClause guard, [NotNull] string? value, string paramName)
        {
            if (value.IsEmpty())
                throw new ArgumentException("مقدار وارد شده نمي‌تواند خالي باشد", paramName);

            return value;
        }

        /// <summary>اگر <paramref name="value"/> منفی باشد ArgumentOutOfRangeException می‌دهد.</summary>
        public static int Negative(this IGuardClause guard, int value, string paramName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(paramName, "مقدار وارد شده نمي‌تواند منفي باشد");

            return value;
        }

        /// <summary>اگر <paramref name="value"/> در بازه‌ی [<paramref name="min"/>, <paramref name="max"/>] نباشد ArgumentOutOfRangeException می‌دهد.</summary>
        public static int OutOfRange(this IGuardClause guard, int value, int min, int max, string paramName)
        {
            if (value < min || value > max)
                throw new ArgumentOutOfRangeException(paramName, $"مقدار وارد شده بايد بين {min} و {max} باشد");

            return value;
        }
    }
}
