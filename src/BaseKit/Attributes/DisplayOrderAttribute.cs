using System;

namespace BaseKit.Attributes
{
    /// <summary>ترتیب نمایش یک پراپرتی در فرم‌های خودکارساز (UI generator)؛ صرفاً metadata، نه اعتبارسنجی.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DisplayOrderAttribute : Attribute
    {
        /// <summary>ترتیب نمایش (عدد کوچیک‌تر زودتر نمایش داده می‌شود).</summary>
        public int Order { get; }

        /// <summary>یک <see cref="DisplayOrderAttribute"/> جدید می‌سازد.</summary>
        public DisplayOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
