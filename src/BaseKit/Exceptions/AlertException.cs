using System;

namespace BaseKit.Exceptions
{
    /// <summary>
    /// استثنایی که پیام آن قابل نمایش مستقیم به کاربر نهایی است
    /// (بر خلاف استثناهای فنی که فقط برای لاگ مناسب‌اند).
    /// </summary>
    public class AlertException : Exception
    {
        public AlertException(string message) : base(message)
        {
        }

        public AlertException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
