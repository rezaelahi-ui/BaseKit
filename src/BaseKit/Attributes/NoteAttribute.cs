using System;

namespace BaseKit.Attributes
{
    /// <summary>
    /// یادداشت/توضیح مستندسازی روی یک متد؛ مناسب تولید مستندات API یا نمایش در ابزارهای داخلی
    /// (نه یک attribute اعتبارسنجی، صرفاً metadata).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NoteAttribute : Attribute
    {
        /// <summary>خلاصه‌ی یک‌خطی درباره‌ی متد.</summary>
        public string Summary { get; }

        /// <summary>توضیح تکمیلی اختیاری.</summary>
        public string? Description { get; set; }

        /// <summary>یک <see cref="NoteAttribute"/> جدید با خلاصه‌ی مشخص می‌سازد.</summary>
        public NoteAttribute(string summary)
        {
            Summary = summary;
        }
    }
}
