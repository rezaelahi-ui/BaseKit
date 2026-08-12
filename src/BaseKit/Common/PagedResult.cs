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
        public IReadOnlyList<T> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

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
