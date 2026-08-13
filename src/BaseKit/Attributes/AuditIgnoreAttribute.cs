using System;

namespace BaseKit.Attributes
{
    /// <summary>
    /// علامت‌گذاری یک پراپرتی برای نادیده‌گرفتن در سیستم audit trail/تاریخچه‌ی تغییرات.
    /// صرفاً metadata است؛ خود BaseKit فعلاً پیاده‌سازی audit ندارد، این attribute برای مصرف در چنین سیستمی (این پروژه یا دیگری) آماده است.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AuditIgnoreAttribute : Attribute
    {
    }
}
