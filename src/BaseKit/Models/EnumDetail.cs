namespace BaseKit.Models
{
    /// <summary>
    /// نمایش یک مقدار Enum به همراه نام قابل‌نمایش و مقدار عددی آن،
    /// مناسب برای پرکردن dropdown/combo در UI.
    /// </summary>
    public class EnumDetail
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}
