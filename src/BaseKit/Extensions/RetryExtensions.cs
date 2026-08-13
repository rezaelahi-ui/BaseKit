using System;
using System.Threading.Tasks;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای اجرای عملیات async با تلاش مجدد (retry).</summary>
    public static class RetryExtensions
    {
        /// <summary>
        /// اجرای یک عملیات async با تلاش مجدد در صورت شکست؛ جایگزین سبک برای Polly در پروژه‌های کوچک.
        /// اگر همه‌ی تلاش‌ها شکست بخورند، آخرین exception در یک AggregateException پرتاب می‌شود.
        /// </summary>
        public static async Task<T> RetryAsync<T>(this Func<Task<T>> action, int retryCount = 3, TimeSpan? delay = null)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));
            if (retryCount < 1) throw new ArgumentOutOfRangeException(nameof(retryCount), "تعداد تلاش بايد حداقل ۱ باشد");

            Exception? lastException = null;

            for (var attempt = 1; attempt <= retryCount; attempt++)
            {
                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (attempt < retryCount && delay.HasValue)
                        await Task.Delay(delay.Value).ConfigureAwait(false);
                }
            }

            throw new AggregateException("تمام تلاش‌ها براي اجراي عمليات ناموفق بود", lastException!);
        }

        /// <summary>نسخه‌ی بدون مقدار بازگشتی <see cref="RetryAsync{T}"/>.</summary>
        public static async Task RetryAsync(this Func<Task> action, int retryCount = 3, TimeSpan? delay = null)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));

            await RetryAsync(async () =>
            {
                await action().ConfigureAwait(false);
                return true;
            }, retryCount, delay).ConfigureAwait(false);
        }
    }
}
