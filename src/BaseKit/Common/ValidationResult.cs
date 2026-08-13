using System.Collections.Generic;

namespace BaseKit.Common
{
    /// <summary>نتیجه‌ی اجرای یک <see cref="Validator{T}"/>؛ شامل تمام خطاهای جمع‌شده (نه فقط اولین خطا).</summary>
    public class ValidationResult
    {
        /// <summary>لیست پیام‌های خطای همه‌ی قوانین ناموفق.</summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>true اگر هیچ خطایی وجود نداشته باشد.</summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>یک <see cref="ValidationResult"/> جدید با لیست خطاهای مشخص می‌سازد.</summary>
        public ValidationResult(IReadOnlyList<string> errors)
        {
            Errors = errors;
        }
    }
}
