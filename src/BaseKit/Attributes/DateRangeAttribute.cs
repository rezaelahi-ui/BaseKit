using System;
using System.ComponentModel.DataAnnotations;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی این‌که تاریخ این فیلد قبل از (یا مساوی) فیلد تاریخ پایان باشد.
    /// روی <see cref="DateTime"/> و رشته‌های تاریخ (مثل فرمت شمسی YYYY/MM/DD، با مقایسه‌ی رشته‌ای) کار می‌کند.
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _endDatePropertyName;
        private readonly bool _allowEqual;

        /// <summary>یک <see cref="DateRangeAttribute"/> جدید می‌سازد.</summary>
        /// <param name="endDatePropertyName">نام پراپرتی تاریخ پایان در همان مدل.</param>
        /// <param name="allowEqual">آیا تاریخ شروع می‌تواند با تاریخ پایان برابر باشد.</param>
        public DateRangeAttribute(string endDatePropertyName, bool allowEqual = true)
        {
            _endDatePropertyName = endDatePropertyName;
            _allowEqual = allowEqual;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            var endPropertyInfo = validationContext.ObjectType.GetProperty(_endDatePropertyName);
            if (endPropertyInfo is null)
            {
                return new ValidationResult(
                    $"فیلد {_endDatePropertyName} یافت نشد",
                    ValidationContextHelpers.GetMemberNames(validationContext));
            }

            var endValue = endPropertyInfo.GetValue(validationContext.ObjectInstance);
            if (endValue is null) return ValidationResult.Success;

            int comparison;
            if (value is DateTime startDate && endValue is DateTime endDate)
            {
                comparison = DateTime.Compare(startDate, endDate);
            }
            else if (value is string startText && endValue is string endText)
            {
                comparison = string.CompareOrdinal(startText, endText);
            }
            else
            {
                return new ValidationResult(
                    "نوع فیلدهای تاریخ برای مقایسه پشتیبانی نمی‌شود",
                    ValidationContextHelpers.GetMemberNames(validationContext));
            }

            var isValid = _allowEqual ? comparison <= 0 : comparison < 0;
            if (isValid) return ValidationResult.Success;

            var message = ErrorMessage ?? $"تاریخ {validationContext.DisplayName} باید قبل از {_endDatePropertyName} باشد";
            return new ValidationResult(message, ValidationContextHelpers.GetMemberNames(validationContext));
        }
    }
}
