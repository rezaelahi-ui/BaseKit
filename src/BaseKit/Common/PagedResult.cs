using System;
using System.Collections.Generic;

namespace BaseKit.Common
{
    /// <summary>
    /// نتیجه‌ی یک صفحه از یک مجموعه‌ی بزرگ‌تر، به‌همراه اطلاعات صفحه‌بندی
    /// (تعداد کل، تعداد صفحات، وجود صفحه بعدی/قبلی).
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>مقادیر همین صفحه.</summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>شماره‌ی صفحه (از ۱ شروع می‌شود).</summary>
        public int PageNumber { get; }

        /// <summary>اندازه‌ی هر صفحه.</summary>
        public int PageSize { get; }

        /// <summary>تعداد کل رکوردها در کل مجموعه (نه فقط این صفحه).</summary>
        public int TotalCount { get; }

        /// <summary>تعداد کل صفحات، محاسبه‌شده از <see cref="TotalCount"/> و <see cref="PageSize"/>.</summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

        /// <summary>آیا صفحه‌ی قبلی وجود دارد.</summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>آیا صفحه‌ی بعدی وجود دارد.</summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>یک <see cref="PagedResult{T}"/> جدید می‌سازد.</summary>
        public PagedResult(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "شماره صفحه بايد حداقل ۱ باشد");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "اندازه صفحه بايد حداقل ۱ باشد");
            if (totalCount < 0)
                throw new ArgumentOutOfRangeException(nameof(totalCount), "تعداد کل نمي‌تواند منفي باشد");

            Items = items ?? throw new ArgumentNullException(nameof(items));
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
