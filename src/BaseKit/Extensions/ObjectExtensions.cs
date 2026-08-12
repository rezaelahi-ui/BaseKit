using System.Diagnostics.CodeAnalysis;

namespace BaseKit.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// بررسی می‌کند رشته null، خالی یا فقط whitespace است.
        /// در صورت false بودن نتیجه، کامپایلر <paramref name="str"/> را non-null در نظر می‌گیرد.
        /// </summary>
        public static bool IsEmpty([NotNullWhen(false)] this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// بررسی می‌کند رشته null، خالی یا فقط whitespace نیست.
        /// در صورت true بودن نتیجه، کامپایلر <paramref name="str"/> را non-null در نظر می‌گیرد.
        /// </summary>
        public static bool IsNotEmpty([NotNullWhen(true)] this string? str)
        {
            return !str.IsEmpty();
        }
    }
}
