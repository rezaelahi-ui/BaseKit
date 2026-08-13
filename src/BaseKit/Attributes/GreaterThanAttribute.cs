using System;
using System.ComponentModel.DataAnnotations;
using BaseKit.Extensions;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی اینکه مقدار عددی پراپرتی بزرگ‌تر یا مساوی یک آستانه‌ی مشخص باشد.
    /// مقدار null نامعتبر تلقی نمی‌شود (الزامی‌بودن مسئولیت <see cref="RequiredAttribute"/> است، نه این attribute).
    /// </summary>
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly double _value;
        private readonly string? _persianPropertyName;

        /// <summary>یک <see cref="GreaterThanAttribute"/> جدید می‌سازد.</summary>
        /// <param name="value">حداقل مقدار مجاز (شامل خودش).</param>
        /// <param name="persianPropertyName">نام فارسی فیلد برای پیام خطا؛ اگر ندهید از <see cref="ValidationContext.DisplayName"/> استفاده می‌شود.</param>
        public GreaterThanAttribute(double value, string? persianPropertyName = null)
        {
            _value = value;
            _persianPropertyName = persianPropertyName;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            var propertyName = _persianPropertyName.IsEmpty() ? validationContext.DisplayName : _persianPropertyName;

            double numericValue;
            try
            {
                numericValue = Convert.ToDouble(value);
            }
            catch (Exception ex) when (ex is FormatException or InvalidCastException)
            {
                return new ValidationResult("فرمت مقدار وارد شده معتبر نیست", ValidationContextHelpers.GetMemberNames(validationContext));
            }

            if (numericValue < _value)
            {
                return new ValidationResult(
                    $"مقدار {propertyName} معتبر نیست و باید بزرگ‌تر یا مساوی {_value} باشد",
                    ValidationContextHelpers.GetMemberNames(validationContext));
            }

            return ValidationResult.Success;
        }
    }
}
