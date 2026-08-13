using System;
using System.Collections.Generic;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای کار با <see cref="Exception"/>.</summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// پیام استثنا را به‌همراه پیام تمام InnerException‌ها در یک رشته جمع می‌کند؛ مناسب برای لاگ کامل.
        /// </summary>
        public static string GetFullMessage(this Exception ex, string separator = " <- ")
        {
            if (ex is null) throw new ArgumentNullException(nameof(ex));

            var messages = new List<string>();
            var current = (Exception?)ex;
            while (current is not null)
            {
                messages.Add(current.Message);
                current = current.InnerException;
            }

            return string.Join(separator, messages);
        }
    }
}
