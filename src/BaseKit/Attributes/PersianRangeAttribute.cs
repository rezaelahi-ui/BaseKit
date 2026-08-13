using System;
using System.ComponentModel.DataAnnotations;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی اینکه مقدار عددی پراپرتی در بازه‌ی [minimum, maximum] باشد، با پیام خطای فارسی.
    /// مقدار null نامعتبر تلقی نمی‌شود (الزامی‌بودن مسئولیت <see cref="RequiredAttribute"/> است، نه این attribute).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PersianRangeAttribute : ValidationAttribute
    {
        private readonly double _minimum;
        private readonly double _maximum;

        /// <summary>یک <see cref="PersianRangeAttribute"/> جدید می‌سازد.</summary>
        public PersianRangeAttribute(double minimum, double maximum, string? message = null)
        {
            _minimum = minimum;
            _maximum = maximum;
            ErrorMessage = message ?? $"مقدار باید بین {minimum} و {maximum} باشد";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            // الزامی‌بودن مسئولیت [Required]/PersianRequired است؛ این attribute فقط بازه را چک می‌کند.
            if (value is null)
                return true;

            double numericValue;
            try
            {
                numericValue = Convert.ToDouble(value);
            }
            catch (Exception ex) when (ex is FormatException or InvalidCastException)
            {
                return false;
            }

            return numericValue >= _minimum && numericValue <= _maximum;
        }
    }
}
