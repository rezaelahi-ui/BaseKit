using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی پسوند فایل آپلودی. روی یک <see cref="string"/> حاوی نام فایل، یا هر آبجکتی که پراپرتی
    /// <c>FileName</c> داشته باشد (duck-typed؛ مثل <c>IFormFile</c> در ASP.NET، بدون وابستگی مستقیم به آن) کار می‌کند.
    /// </summary>
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        /// <summary>یک <see cref="AllowedExtensionsAttribute"/> جدید می‌سازد.</summary>
        /// <param name="extensions">پسوندهای مجاز، با یا بدون نقطه (مثلاً "jpg" یا ".jpg").</param>
        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions.Select(e => e.StartsWith(".", StringComparison.Ordinal) ? e : "." + e).ToArray();
            ErrorMessage = $"پسوند فایل باید یکی از موارد {string.Join(", ", _extensions)} باشد";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (value is null) return true;

            var fileName = value as string ?? ExtractFileName(value);
            if (string.IsNullOrWhiteSpace(fileName)) return true;

            var extension = Path.GetExtension(fileName);
            return _extensions.Any(e => string.Equals(e, extension, StringComparison.OrdinalIgnoreCase));
        }

        private static string? ExtractFileName(object value)
        {
            var fileNameProperty = value.GetType().GetProperty("FileName");
            return fileNameProperty?.GetValue(value) as string;
        }
    }
}
