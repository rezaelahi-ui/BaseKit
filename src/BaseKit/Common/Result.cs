using System;

namespace BaseKit.Common
{
    /// <summary>
    /// نتیجه‌ی یک عملیات که می‌تواند موفق (با مقدار) یا ناموفق (با پیام خطا) باشد،
    /// بدون نیاز به throw کردن exception برای مسیرهای خطای قابل‌پیش‌بینی.
    /// </summary>
    public class Result<T>
    {
        /// <summary>آیا عملیات موفق بوده است.</summary>
        public bool IsSuccess { get; }

        /// <summary>آیا عملیات ناموفق بوده است.</summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>مقدار نتیجه، فقط در حالت موفق معتبر است.</summary>
        public T? Value { get; }

        /// <summary>پیام خطا، فقط در حالت ناموفق مقداردهی می‌شود.</summary>
        public string? Error { get; }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        /// <summary>ساخت یک نتیجه‌ی موفق با مقدار مشخص.</summary>
        public static Result<T> Success(T value) => new(true, value, null);

        /// <summary>ساخت یک نتیجه‌ی ناموفق با پیام خطای مشخص.</summary>
        public static Result<T> Failure(string error) => new(false, default, error);

        /// <summary>در صورت موفق‌بودن Value را برمی‌گرداند، وگرنه InvalidOperationException می‌دهد.</summary>
        public T GetValueOrThrow()
        {
            if (IsFailure)
                throw new InvalidOperationException(Error ?? "عمليات ناموفق بود");

            return Value!;
        }
    }
}
