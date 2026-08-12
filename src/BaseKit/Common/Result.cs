using System;

namespace BaseKit.Common
{
    /// <summary>
    /// نتیجه‌ی یک عملیات که می‌تواند موفق (با مقدار) یا ناموفق (با پیام خطا) باشد،
    /// بدون نیاز به throw کردن exception برای مسیرهای خطای قابل‌پیش‌بینی.
    /// </summary>
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public string? Error { get; }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);

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
