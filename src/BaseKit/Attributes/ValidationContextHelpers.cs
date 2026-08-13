using System.ComponentModel.DataAnnotations;

namespace BaseKit.Attributes
{
    /// <summary>متدهای کمکی مشترک بین attributeهای اعتبارسنجی این پروژه.</summary>
    internal static class ValidationContextHelpers
    {
        /// <summary>آرایه‌ی نام عضو برای ساخت <see cref="ValidationResult"/>، یا null اگر MemberName مشخص نباشد.</summary>
        public static string[]? GetMemberNames(ValidationContext validationContext)
            => validationContext.MemberName is null ? null : new[] { validationContext.MemberName };
    }
}
