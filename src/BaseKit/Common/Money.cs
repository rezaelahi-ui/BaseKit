using System;
using BaseKit.Extensions;

namespace BaseKit.Common
{
    /// <summary>
    /// یک مبلغ پولی به‌همراه واحد آن؛ از جمع/تفریق/مقایسه‌ی اشتباهِ دو واحد پول متفاوت جلوگیری می‌کند
    /// و جایگزین ایمن‌تری برای نگه‌داشتن مبلغ به‌صورت decimal خام است.
    /// </summary>
    public readonly struct Money : IEquatable<Money>, IComparable<Money>
    {
        /// <summary>مبلغ عددی.</summary>
        public decimal Amount { get; }

        /// <summary>کد واحد پول (نرمال‌شده به حروف بزرگ، مثل IRR/USD).</summary>
        public string Currency { get; }

        /// <summary>یک <see cref="Money"/> جدید می‌سازد.</summary>
        public Money(decimal amount, string currency)
        {
            if (currency.IsEmpty())
                throw new ArgumentNullException(nameof(currency), "واحد پول نمي‌تواند خالي باشد");

            Amount = amount;
            Currency = currency.Trim().ToUpperInvariant();
        }

        /// <summary>مبلغ صفر با واحد پول مشخص‌شده.</summary>
        public static Money Zero(string currency) => new(0, currency);

        /// <summary>جمع دو مبلغ با واحد پول یکسان؛ در غیر این صورت <see cref="InvalidOperationException"/> می‌دهد.</summary>
        public static Money operator +(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return new Money(a.Amount + b.Amount, a.Currency);
        }

        /// <summary>تفریق دو مبلغ با واحد پول یکسان؛ در غیر این صورت <see cref="InvalidOperationException"/> می‌دهد.</summary>
        public static Money operator -(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return new Money(a.Amount - b.Amount, a.Currency);
        }

        /// <summary>ضرب مبلغ در یک ضریب عددی (مثلاً برای نرخ ارز یا تخفیف).</summary>
        public static Money operator *(Money money, decimal factor) => new(money.Amount * factor, money.Currency);

        /// <summary>برابری بر اساس مبلغ و واحد پول.</summary>
        public static bool operator ==(Money a, Money b) => a.Equals(b);

        /// <summary>نابرابری بر اساس مبلغ و واحد پول.</summary>
        public static bool operator !=(Money a, Money b) => !a.Equals(b);

        /// <summary>مقایسه‌ی کمتر بودن؛ فقط برای واحد پول یکسان.</summary>
        public static bool operator <(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount < b.Amount;
        }

        /// <summary>مقایسه‌ی بیشتر بودن؛ فقط برای واحد پول یکسان.</summary>
        public static bool operator >(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount > b.Amount;
        }

        /// <summary>مقایسه‌ی کمتر یا مساوی بودن؛ فقط برای واحد پول یکسان.</summary>
        public static bool operator <=(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount <= b.Amount;
        }

        /// <summary>مقایسه‌ی بیشتر یا مساوی بودن؛ فقط برای واحد پول یکسان.</summary>
        public static bool operator >=(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return a.Amount >= b.Amount;
        }

        private static void EnsureSameCurrency(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException(
                    $"نمي‌توان دو واحد پول متفاوت ({a.Currency} و {b.Currency}) را با هم جمع/تفريق/مقايسه کرد");
        }

        /// <inheritdoc/>
        public bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Money other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (Amount, Currency).GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => $"{Amount.ToSeparatedString()} {Currency}";

        /// <summary>مقایسه با یک <see cref="Money"/> دیگر با واحد پول یکسان.</summary>
        public int CompareTo(Money other)
        {
            EnsureSameCurrency(this, other);
            return Amount.CompareTo(other.Amount);
        }
    }
}
