using System;

namespace BaseKit.Models
{
    /// <summary>
    /// یک هفته (شروع از شنبه، کوتاه‌شده به بازه‌ی ماه) در تقسیم‌بندی هفته‌های یک ماه شمسی
    /// (خروجی <see cref="Extensions.DateExtensions.GetWeeksOfShamsiMonth"/>).
    /// </summary>
    public class WeekInfo
    {
        /// <summary>شماره‌ی هفته درون ماه (از ۱ شروع می‌شود).</summary>
        public int WeekNumber { get; set; }

        /// <summary>تاریخ میلادی شروع هفته (در اولین/آخرین هفته‌ی ماه، به بازه‌ی ماه محدود می‌شود).</summary>
        public DateTime StartDate { get; set; }

        /// <summary>تاریخ میلادی پایان هفته.</summary>
        public DateTime EndDate { get; set; }

        /// <summary>تاریخ شمسی شروع هفته.</summary>
        public string StartDateShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی پایان هفته.</summary>
        public string EndDateShamsi { get; set; } = string.Empty;

        /// <summary>آیا این هفته تا اکنون به پایان رسیده است.</summary>
        public bool IsComplete { get; set; }
    }
}
