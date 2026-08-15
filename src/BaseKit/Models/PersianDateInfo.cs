using System;

namespace BaseKit.Models
{
    /// <summary>
    /// اطلاعات کامل یک تاریخ در تقویم شمسی: سال/ماه/روز/نام‌ها، به‌همراه بازه‌ی شروع و پایان
    /// ماه، فصل و سالی که آن تاریخ در آن قرار دارد (خروجی <see cref="Extensions.DateExtensions.GetPersianDateInfo"/>).
    /// </summary>
    public class PersianDateInfo
    {
        /// <summary>سال شمسی.</summary>
        public int Year { get; set; }

        /// <summary>ماه شمسی (۱ تا ۱۲).</summary>
        public int Month { get; set; }

        /// <summary>روز ماه شمسی.</summary>
        public int Day { get; set; }

        /// <summary>روز هفته (بر اساس <see cref="System.DayOfWeek"/>؛ در تقویم میلادی و شمسی یکسان است).</summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>نام فارسی روز هفته (مثل «شنبه»).</summary>
        public string DayName { get; set; } = string.Empty;

        /// <summary>نام فارسی ماه (مثل «فروردین»).</summary>
        public string MonthName { get; set; } = string.Empty;

        /// <summary>نام فارسی فصل (مثل «بهار»).</summary>
        public string SeasonName { get; set; } = string.Empty;

        /// <summary>تعداد روزهای ماه جاری.</summary>
        public int DaysInMonth { get; set; }

        /// <summary>تعداد روزهای سال جاری (۳۶۵ یا ۳۶۶ در سال کبیسه).</summary>
        public int DaysInYear { get; set; }

        /// <summary>خود تاریخ به‌صورت رشته‌ی شمسی YYYY/MM/DD.</summary>
        public string ShamsiDate { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی شروع ماه جاری.</summary>
        public string MonthStartShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی پایان ماه جاری.</summary>
        public string MonthEndShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی شروع فصل جاری.</summary>
        public string SeasonStartShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی پایان فصل جاری.</summary>
        public string SeasonEndShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی شروع سال جاری.</summary>
        public string YearStartShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی پایان سال جاری.</summary>
        public string YearEndShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ میلادی شروع ماه جاری.</summary>
        public DateTime MonthStartDate { get; set; }

        /// <summary>تاریخ میلادی پایان ماه جاری.</summary>
        public DateTime MonthEndDate { get; set; }

        /// <summary>تاریخ میلادی شروع فصل جاری.</summary>
        public DateTime SeasonStartDate { get; set; }

        /// <summary>تاریخ میلادی پایان فصل جاری.</summary>
        public DateTime SeasonEndDate { get; set; }

        /// <summary>تاریخ میلادی شروع سال جاری.</summary>
        public DateTime YearStartDate { get; set; }

        /// <summary>تاریخ میلادی پایان سال جاری.</summary>
        public DateTime YearEndDate { get; set; }
    }
}
