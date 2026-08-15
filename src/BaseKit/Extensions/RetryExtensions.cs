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

        /// <summary>
        /// مثل <see cref="RetryAsync{T}(Func{Task{T}}, int, TimeSpan?)"/>، با این تفاوت که فاصله‌ی بین تلاش‌ها
        /// به‌جای ثابت‌بودن، هر بار در <paramref name="backoffFactor"/> ضرب می‌شود (exponential backoff)
        /// و کمی نویز تصادفی (jitter) به آن اضافه می‌شود؛ مناسب صدا زدن APIهای بیرونی.
        /// </summary>
        public static async Task<T> RetryWithBackoffAsync<T>(
            this Func<Task<T>> action,
            int retryCount = 3,
            TimeSpan? initialDelay = null,
            double backoffFactor = 2.0,
            TimeSpan? maxDelay = null)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));
            if (retryCount < 1) throw new ArgumentOutOfRangeException(nameof(retryCount), "تعداد تلاش بايد حداقل ۱ باشد");
            if (backoffFactor < 1) throw new ArgumentOutOfRangeException(nameof(backoffFactor), "ضريب backoff بايد حداقل ۱ باشد");

            var delay = initialDelay ?? TimeSpan.FromMilliseconds(200);
            var random = new Random();
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
                    if (attempt >= retryCount) break;

                    var jitterMs = random.Next(0, (int)Math.Max(1, delay.TotalMilliseconds * 0.2));
                    await Task.Delay(delay + TimeSpan.FromMilliseconds(jitterMs)).ConfigureAwait(false);

                    var nextDelay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * backoffFactor);
                    delay = maxDelay.HasValue && nextDelay > maxDelay.Value ? maxDelay.Value : nextDelay;
                }
            }

            throw new AggregateException("تمام تلاش‌ها براي اجراي عمليات ناموفق بود", lastException!);
        }

        /// <summary>نسخه‌ی بدون مقدار بازگشتی <see cref="RetryWithBackoffAsync{T}"/>.</summary>
        public static async Task RetryWithBackoffAsync(
            this Func<Task> action,
            int retryCount = 3,
            TimeSpan? initialDelay = null,
            double backoffFactor = 2.0,
            TimeSpan? maxDelay = null)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));

            await RetryWithBackoffAsync(async () =>
            {
                await action().ConfigureAwait(false);
                return true;
            }, retryCount, initialDelay, backoffFactor, maxDelay).ConfigureAwait(false);
        }
    }
}
