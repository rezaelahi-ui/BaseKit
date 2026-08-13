namespace BaseKit.Models
{
    /// <summary>
    /// نمایش یک مقدار Enum به همراه نام قابل‌نمایش و مقدار عددی آن،
    /// مناسب برای پرکردن dropdown/combo در UI.
    /// </summary>
    public class EnumDetail
    {
        /// <summary>نام قابل‌نمایش مقدار Enum (خروجی <see cref="Extensions.EnumExtensions.Humanize"/>).</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>مقدار عددی Enum.</summary>
        public int Value { get; set; }
    }
}
