using System.ComponentModel.DataAnnotations;
using BaseKit.Extensions;

namespace BaseKit.Attributes
{
    /// <summary>
    /// اعتبارسنجی حداکثر حجم فایل. روی <see cref="long"/>/<see cref="int"/> (بایت)، یا هر آبجکتی که پراپرتی
    /// <c>Length</c> داشته باشد (duck-typed؛ مثل <c>IFormFile</c> یا <see cref="System.IO.Stream"/>) کار می‌کند.
    /// </summary>
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxSizeInBytes;

        /// <summary>یک <see cref="MaxFileSizeAttribute"/> جدید می‌سازد.</summary>
        public MaxFileSizeAttribute(long maxSizeInBytes)
        {
            _maxSizeInBytes = maxSizeInBytes;
            ErrorMessage = $"حجم فایل نباید بیشتر از {maxSizeInBytes.ToSeparatedString()} بایت باشد";
        }

        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (value is null) return true;

            var length = ExtractLength(value);
            return length is null || length <= _maxSizeInBytes;
        }

        private static long? ExtractLength(object value)
        {
            if (value is long longValue) return longValue;
            if (value is int intValue) return intValue;

            var lengthProperty = value.GetType().GetProperty("Length");
            return lengthProperty?.GetValue(value) switch
            {
                long l => l,
                int i => i,
                _ => null,
            };
        }
    }
}
