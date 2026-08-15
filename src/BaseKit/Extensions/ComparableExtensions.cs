using System;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension عمومی برای مقادیر قابل‌مقایسه (<see cref="IComparable{T}"/>): عدد، تاریخ، رشته، <see cref="BaseKit.Common.Money"/> و ...</summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// بررسی می‌کند مقدار در بازه‌ی [<paramref name="min"/>, <paramref name="max"/>] قرار دارد (هر دو سر بازه شامل می‌شوند).
        /// اگر <paramref name="min"/> بزرگ‌تر از <paramref name="max"/> باشد، همیشه false برمی‌گرداند.
        /// </summary>
        public static bool Between<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (min.CompareTo(max) > 0) return false;

            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }
    }
}
