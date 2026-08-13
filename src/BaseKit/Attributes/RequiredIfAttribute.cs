using System.ComponentModel.DataAnnotations;

namespace BaseKit.Attributes
{
    /// <summary>
    /// یک فیلد را فقط زمانی الزامی می‌کند که فیلد دیگری از همان مدل مقدار مشخصی داشته باشد
    /// (مثلاً <c>[RequiredIf(nameof(HasDiscount), true)]</c> روی <c>DiscountAmount</c>).
    /// معادلی در DataAnnotations استاندارد ندارد.
    /// </summary>
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;
        private readonly object? _otherPropertyValue;

        /// <summary>یک <see cref="RequiredIfAttribute"/> جدید می‌سازد.</summary>
        /// <param name="otherProperty">نام پراپرتی شرط، از همان مدل.</param>
        /// <param name="otherPropertyValue">مقداری که اگر <paramref name="otherProperty"/> داشته باشد، این فیلد الزامی می‌شود.</param>
        public RequiredIfAttribute(string otherProperty, object? otherPropertyValue)
        {
            _otherProperty = otherProperty;
            _otherPropertyValue = otherPropertyValue;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherPropertyInfo is null)
            {
                return new ValidationResult(
                    $"فیلد {_otherProperty} یافت نشد",
                    ValidationContextHelpers.GetMemberNames(validationContext));
            }

            var actualOtherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
            var conditionMet = Equals(actualOtherValue, _otherPropertyValue);

            if (!conditionMet) return ValidationResult.Success;

            var isEmpty = value is null || (value is string str && string.IsNullOrWhiteSpace(str));
            if (!isEmpty) return ValidationResult.Success;

            var message = ErrorMessage ?? $"وارد کردن {validationContext.DisplayName} الزامی است";
            return new ValidationResult(message, ValidationContextHelpers.GetMemberNames(validationContext));
        }
    }
}
