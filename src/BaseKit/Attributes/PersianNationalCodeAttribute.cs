using System.ComponentModel.DataAnnotations;
using BaseKit.Extensions;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی کد ملی ایرانی با الگوریتم چک‌دیجیت (بر پایه‌ی <see cref="ValidationExtensions.IsValidNationalCode"/>).
    /// مقدار null نامعتبر تلقی نمی‌شود (الزامی‌بودن مسئولیت <see cref="RequiredAttribute"/> است).
    /// </summary>
    public class PersianNationalCodeAttribute : ValidationAttribute
    {
        /// <summary>یک <see cref="PersianNationalCodeAttribute"/> جدید می‌سازد.</summary>
        public PersianNationalCodeAttribute()
        {
            ErrorMessage = "کد ملی وارد شده معتبر نیست";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (value is null) return true;

            return value is string str && str.IsValidNationalCode();
        }
    }
}
