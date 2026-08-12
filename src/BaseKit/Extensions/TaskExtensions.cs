using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaseKit.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// اجرای یک Task با محدودیت زمانی؛ اگر Task در بازه‌ی مشخص تمام نشود TimeoutException پرتاب می‌شود.
        /// </summary>
        public static async Task WithTimeout(this Task task, TimeSpan timeout)
        {
            if (task is null) throw new ArgumentNullException(nameof(task));

            using var cts = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, cts.Token);
            var completed = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

            if (completed == delayTask)
                throw new TimeoutException($"عمليات در بازه {timeout} به پايان نرسيد");

            cts.Cancel();
            await task.ConfigureAwait(false);
        }

        /// <summary>
        /// اجرای یک Task&lt;T&gt; با محدودیت زمانی؛ اگر Task در بازه‌ی مشخص تمام نشود TimeoutException پرتاب می‌شود.
        /// </summary>
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            if (task is null) throw new ArgumentNullException(nameof(task));

            using var cts = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, cts.Token);
            var completed = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

            if (completed == delayTask)
                throw new TimeoutException($"عمليات در بازه {timeout} به پايان نرسيد");

            cts.Cancel();
            return await task.ConfigureAwait(false);
        }
    }
}
