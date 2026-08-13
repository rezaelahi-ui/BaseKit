using System.ComponentModel.DataAnnotations;
using BaseKit.Extensions;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی شماره شبا (بر پایه‌ی <see cref="ValidationExtensions.IsValidIban"/> با الگوریتم mod-97).
    /// مقدار null نامعتبر تلقی نمی‌شود (الزامی‌بودن مسئولیت <see cref="RequiredAttribute"/> است).
    /// </summary>
    public class IranianIbanAttribute : ValidationAttribute
    {
        /// <summary>یک <see cref="IranianIbanAttribute"/> جدید می‌سازد.</summary>
        public IranianIbanAttribute()
        {
            ErrorMessage = "شماره شبا وارد شده معتبر نیست";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (value is null) return true;

            return value is string str && str.IsValidIban();
        }
    }
}
