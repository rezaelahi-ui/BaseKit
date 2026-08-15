using System;

namespace BaseKit.Models
{
    /// <summary>
    /// یک فصل شمسی درون یک سال (خروجی <see cref="Extensions.DateExtensions.GetSeasonsOfShamsiYear"/>).
    /// </summary>
    public class SeasonInfo
    {
        /// <summary>شماره‌ی فصل (۱ = بهار تا ۴ = زمستان).</summary>
        public int SeasonNumber { get; set; }

        /// <summary>نام فارسی فصل.</summary>
        public string SeasonName { get; set; } = string.Empty;

        /// <summary>تاریخ میلادی شروع فصل.</summary>
        public DateTime StartDate { get; set; }

        /// <summary>تاریخ میلادی پایان فصل.</summary>
        public DateTime EndDate { get; set; }

        /// <summary>تاریخ شمسی شروع فصل.</summary>
        public string StartDateShamsi { get; set; } = string.Empty;

        /// <summary>تاریخ شمسی پایان فصل.</summary>
        public string EndDateShamsi { get; set; } = string.Empty;

        /// <summary>آیا این فصل تا اکنون به پایان رسیده است.</summary>
        public bool IsComplete { get; set; }
    }
}
