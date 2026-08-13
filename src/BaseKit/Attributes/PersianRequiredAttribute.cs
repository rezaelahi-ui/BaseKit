using System;
using System.ComponentModel.DataAnnotations;

namespace BaseKit.Attributes
{
    /// <summary>نسخه‌ی فارسی <see cref="RequiredAttribute"/>؛ رشته‌ی خالی/فقط-whitespace را هم نامعتبر می‌داند.</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class PersianRequiredAttribute : RequiredAttribute
    {
        /// <summary>یک <see cref="PersianRequiredAttribute"/> جدید می‌سازد.</summary>
        /// <param name="message">نام فارسی فیلد برای پیام خطا؛ اگر ندهید پیام عمومی استفاده می‌شود.</param>
        public PersianRequiredAttribute(string? message = null)
        {
            ErrorMessage = message is null
                ? "وارد کردن این فیلد الزامی است"
                : $"وارد کردن {message} الزامی است";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            return value is not null && !string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}
