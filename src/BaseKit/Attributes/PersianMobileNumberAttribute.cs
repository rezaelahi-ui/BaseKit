using System.ComponentModel.DataAnnotations;
using BaseKit.Extensions;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی شماره موبایل ایران (بر پایه‌ی <see cref="ValidationExtensions.IsValidMobileNumber"/>).
    /// مقدار null نامعتبر تلقی نمی‌شود (الزامی‌بودن مسئولیت <see cref="RequiredAttribute"/> است).
    /// </summary>
    public class PersianMobileNumberAttribute : ValidationAttribute
    {
        /// <summary>یک <see cref="PersianMobileNumberAttribute"/> جدید می‌سازد.</summary>
        public PersianMobileNumberAttribute()
        {
            ErrorMessage = "شماره موبایل وارد شده معتبر نیست";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (value is null) return true;

            return value is string str && str.IsValidMobileNumber();
        }
    }
}
