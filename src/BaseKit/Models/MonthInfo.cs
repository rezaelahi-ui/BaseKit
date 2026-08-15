using System;

namespace BaseKit.Models
{
    /// <summary>
    /// یک ماه شمسی درون یک فصل (خروجی <see cref="Extensions.DateExtensions.GetMonthsOfShamsiSeason"/>).
    /// </summary>
    public class MonthInfo
    {
        /// <summary>سال شمسی این ماه.</summary>
        public int Year { get; set; }

        /// <summary>شماره‌ی ماه شمسی (۱ تا ۱۲).</summary>
        public int MonthNumber { get; set; }

        /// <summary>نام فارسی ماه.</summary>
        public string MonthName { get; set; } = string.Empty;

        /// <summary>تاریخ میلادی شروع ماه.</summary>
        public DateTime StartDate { get; set; }

        /// <summary>تاریخ میلادی پایان ماه.</summary>
        public DateTime EndDate { get; set; }

        /// <summary>تاریخ شمسی شروع ماه.</summary>
        public string StartDateShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی پایان ماه.</summary>
        public string EndDateShamsi { get; set; } = string.Empty;

        /// <summary>آیا این ماه تا اکنون به پایان رسیده است.</summary>
        public bool IsComplete { get; set; }
    }
}
