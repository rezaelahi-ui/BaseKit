using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای <see cref="Task"/>/<see cref="Task{TResult}"/>.</summary>
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

        /// <summary>
        /// منتظر تمام Taskها می‌ماند حتی اگر بعضی fail بشوند؛ بر خلاف <see cref="Task.WhenAll(Task[])"/>
        /// که فقط اولین exception را پرتاب می‌کند، همه‌ی exceptionهای Taskهای fail‌شده را در یک
        /// <see cref="AggregateException"/> جمع می‌کند.
        /// </summary>
        public static async Task WhenAllSafe(this IEnumerable<Task> tasks)
        {
            if (tasks is null) throw new ArgumentNullException(nameof(tasks));

            var materialized = tasks as IReadOnlyCollection<Task> ?? tasks.ToList();

            try
            {
                await Task.WhenAll(materialized).ConfigureAwait(false);
            }
            catch
            {
                var exceptions = materialized
                    .Where(t => t.IsFaulted && t.Exception is not null)
                    .SelectMany(t => t.Exception!.InnerExceptions)
                    .ToList();

                if (exceptions.Count > 0)
                    throw new AggregateException("يک يا چند Task با خطا مواجه شدند", exceptions);

                throw;
            }
        }

        /// <summary>نسخه‌ی <see cref="Task{TResult}"/>ی <see cref="WhenAllSafe(IEnumerable{Task})"/>؛ نتیجه‌ی همه‌ی Taskهای موفق را برمی‌گرداند.</summary>
        public static async Task<TResult[]> WhenAllSafe<TResult>(this IEnumerable<Task<TResult>> tasks)
        {
            if (tasks is null) throw new ArgumentNullException(nameof(tasks));

            var materialized = tasks as IReadOnlyCollection<Task<TResult>> ?? tasks.ToList();

            try
            {
                return await Task.WhenAll(materialized).ConfigureAwait(false);
            }
            catch
            {
                var exceptions = materialized
                    .Where(t => t.IsFaulted && t.Exception is not null)
                    .SelectMany(t => t.Exception!.InnerExceptions)
                    .ToList();

                if (exceptions.Count > 0)
                    throw new AggregateException("يک يا چند Task با خطا مواجه شدند", exceptions);

                throw;
            }
        }
    }
}
