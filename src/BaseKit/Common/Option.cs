using System;

namespace BaseKit.Common
{
    /// <summary>
    /// مقداری که ممکن است وجود داشته باشد یا نه؛ مکمل <see cref="Result{T}"/> برای جایی که فقط
    /// «هست/نیست» مهم است، نه یک پیام خطای مشخص.
    /// </summary>
    public readonly struct Option<T>
    {
        private readonly T? _value;

        /// <summary>آیا مقدار وجود دارد.</summary>
        public bool HasValue { get; }

        private Option(bool hasValue, T? value)
        {
            HasValue = hasValue;
            _value = value;
        }

        /// <summary>یک <see cref="Option{T}"/> دارای مقدار.</summary>
        public static Option<T> Some(T value) => new(true, value);

        /// <summary>یک <see cref="Option{T}"/> بدون مقدار.</summary>
        public static Option<T> None() => new(false, default);

        /// <summary>ساخت یک <see cref="Option{T}"/> از یک مقدار nullable؛ null به <see cref="None"/> نگاشت می‌شود.</summary>
        public static Option<T> FromNullable(T? value) => value is null ? None() : Some(value);

        /// <summary>در صورت وجود مقدار آن را برمی‌گرداند، وگرنه <see cref="InvalidOperationException"/> می‌دهد.</summary>
        public T GetValueOrThrow()
        {
            if (!HasValue)
                throw new InvalidOperationException("مقداري وجود ندارد");

            return _value!;
        }

        /// <summary>در صورت وجود مقدار آن را برمی‌گرداند، وگرنه <paramref name="defaultValue"/> را.</summary>
        public T GetValueOrDefault(T defaultValue) => HasValue ? _value! : defaultValue;

        /// <summary>تلاش برای دریافت مقدار به‌سبک TryGet.</summary>
        public bool TryGetValue(out T value)
        {
            value = _value!;
            return HasValue;
        }

        /// <summary>اجرای یکی از دو تابع بسته به وجود یا نبود مقدار و برگرداندن نتیجه‌ی آن.</summary>
        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (some is null) throw new ArgumentNullException(nameof(some));
            if (none is null) throw new ArgumentNullException(nameof(none));

            return HasValue ? some(_value!) : none();
        }
    }
}
