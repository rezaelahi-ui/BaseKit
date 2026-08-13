using System;

namespace BaseKit.Exceptions
{
    /// <summary>
    /// استثنایی که پیام آن قابل نمایش مستقیم به کاربر نهایی است
    /// (بر خلاف استثناهای فنی که فقط برای لاگ مناسب‌اند).
    /// </summary>
    public class AlertException : Exception
    {
        /// <summary>یک <see cref="AlertException"/> جدید با پیام قابل‌نمایش به کاربر می‌سازد.</summary>
        public AlertException(string message) : base(message)
        {
        }

        /// <summary>یک <see cref="AlertException"/> جدید با پیام قابل‌نمایش و استثنای داخلی می‌سازد.</summary>
        public AlertException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
