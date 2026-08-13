using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی یک پراپرتی رشته‌ای با Regex و پیام خطای دلخواه.
    /// اگر پراپرتی nullable باشد (<c>int?</c> یا در .NET 6+ همچنین <c>string?</c>)، مقدار خالی/null معتبر در نظر گرفته می‌شود.
    /// </summary>
    public class PersianRegularExpressionAttribute : RegularExpressionAttribute
    {
        private readonly string _message;
        private readonly string _regex;

        /// <summary>یک <see cref="PersianRegularExpressionAttribute"/> جدید با الگوی regex و پیام خطا می‌سازد.</summary>
        public PersianRegularExpressionAttribute(string regex, string message) : base(regex)
        {
            _regex = regex;
            _message = message;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName!);
            var isNullable = IsNullableProperty(propertyInfo);

            var stringValue = value?.ToString();

            // مقدار خالی یا null
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                if (isNullable)
                    return ValidationResult.Success;

                return new ValidationResult(
                    $"مقدار {validationContext.DisplayName} نمی‌تواند خالی باشد",
                    GetMemberNames(validationContext));
            }

            // اعتبارسنجی Regex
            if (!Regex.IsMatch(stringValue, _regex))
            {
                var errorMessage = !string.IsNullOrEmpty(_message)
                    ? _message
                    : $"قالب {validationContext.DisplayName} درست نیست";

                return new ValidationResult(errorMessage, GetMemberNames(validationContext));
            }

            return ValidationResult.Success;
        }

        private static string[]? GetMemberNames(ValidationContext validationContext)
            => validationContext.MemberName is null ? null : new[] { validationContext.MemberName };

        private static bool IsNullableProperty(PropertyInfo? propertyInfo)
        {
            if (propertyInfo is null) return false;

            // Nullable<T> مثل int? یا DateTime? — روی همه‌ی target frameworkها کار می‌کند
            if (propertyInfo.PropertyType.IsGenericType &&
                propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;

#if NET6_0_OR_GREATER
            // Reference typeهای nullable annotation‌دار مثل string?؛ فقط در .NET 6+ با NullabilityInfoContext قابل تشخیص است.
            var nullabilityContext = new NullabilityInfoContext();
            var nullabilityInfo = nullabilityContext.Create(propertyInfo);
            return nullabilityInfo.WriteState == NullabilityState.Nullable;
#else
            return false;
#endif
        }
    }
}
