using System;

namespace BaseKit.Exceptions
{
    /// <summary>
    /// استثنایی برای خطاهای اعتبارسنجی ورودی کاربر در APIها؛ معمولاً باید به کد وضعیت HTTP 400 (Bad Request) نگاشت شود
    /// (بر خلاف <see cref="AlertException"/> که برای پیام‌های عمومی قابل‌نمایش به کاربر است، نه لزوماً خطای اعتبارسنجی API).
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>یک <see cref="BadRequestException"/> جدید با پیام خطا می‌سازد.</summary>
        public BadRequestException(string message) : base(message)
        {
        }

        /// <summary>یک <see cref="BadRequestException"/> جدید با پیام خطا و استثنای داخلی می‌سازد.</summary>
        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
