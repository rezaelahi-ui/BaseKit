using System;
using System.ComponentModel.DataAnnotations;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی رابطه‌ی مقایسه‌ای (&lt;, &lt;=, &gt;, &gt;=, ==, !=) بین یک فیلد و فیلد دیگری از همان مدل.
    /// بر خلاف <see cref="CompareAttribute"/> استاندارد (که فقط برابری را چک می‌کند)، همه‌ی رابطه‌های <see cref="CompareType"/> را پشتیبانی می‌کند.
    /// فیلدهای مقایسه‌شونده باید <see cref="IComparable"/> باشند (عدد، تاریخ، رشته و ...).
    /// </summary>
    public class CompareToAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;
        private readonly CompareType _comparison;

        /// <summary>یک <see cref="CompareToAttribute"/> جدید می‌سازد.</summary>
        /// <param name="otherProperty">نام پراپرتی دیگری از همان مدل که باید با آن مقایسه شود.</param>
        /// <param name="comparison">نوع رابطه‌ی مورد انتظار.</param>
        public CompareToAttribute(string otherProperty, CompareType comparison)
        {
            _otherProperty = otherProperty;
            _comparison = comparison;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            if (value is not IComparable comparable)
            {
                return new ValidationResult(
                    "نوع فیلد برای مقایسه پشتیبانی نمی‌شود",
                    ValidationContextHelpers.GetMemberNames(validationContext));
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherPropertyInfo is null)
            {
                return new ValidationResult(
                    $"فیلد {_otherProperty} یافت نشد",
                    ValidationContextHelpers.GetMemberNames(validationContext));
            }

            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
            if (otherValue is null) return ValidationResult.Success;

            var comparisonResult = comparable.CompareTo(otherValue);
            var isValid = _comparison switch
            {
                CompareType.Equal => comparisonResult == 0,
                CompareType.NotEqual => comparisonResult != 0,
                CompareType.LessThan => comparisonResult < 0,
                CompareType.LessThanOrEqual => comparisonResult <= 0,
                CompareType.GreaterThan => comparisonResult > 0,
                CompareType.GreaterThanOrEqual => comparisonResult >= 0,
                _ => throw new ArgumentOutOfRangeException(nameof(_comparison), _comparison, null),
            };

            if (isValid) return ValidationResult.Success;

            var message = ErrorMessage ?? $"مقدار {validationContext.DisplayName} با {_otherProperty} رابطه‌ی معتبر ندارد";
            return new ValidationResult(message, ValidationContextHelpers.GetMemberNames(validationContext));
        }
    }
}
