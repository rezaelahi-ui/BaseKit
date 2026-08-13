using System;
using System.Collections.Generic;
using BaseKit.Extensions;

namespace BaseKit.Common
{
    /// <summary>
    /// اعتبارسنجی fluent با جمع‌آوری همه‌ی خطاها (بر خلاف <see cref="Guards.Guard"/> که در اولین خطا throw می‌کند).
    /// مناسب برای فرم‌ها/DTOهایی که باید همه‌ی خطاهای ورودی یک‌جا به کاربر نمایش داده شوند.
    /// </summary>
    /// <example>
    /// var result = Validator&lt;UserDto&gt;.For(dto)
    ///     .Rule(x => x.Name.IsNotEmpty(), "نام الزامی است")
    ///     .Rule(x => x.Mobile.IsValidMobileNumber(), "موبایل نامعتبر است")
    ///     .Validate();
    /// </example>
    public class Validator<T>
    {
        private readonly T _instance;
        private readonly List<(Func<T, bool> Predicate, string Message)> _rules = new();

        private Validator(T instance)
        {
            _instance = instance;
        }

        /// <summary>شروع یک زنجیره‌ی اعتبارسنجی fluent برای یک instance مشخص.</summary>
        public static Validator<T> For(T instance)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance));
            return new Validator<T>(instance);
        }

        /// <summary>افزودن یک قانون؛ اگر <paramref name="predicate"/> برای instance فعلی false برگرداند، <paramref name="message"/> به لیست خطاها اضافه می‌شود.</summary>
        public Validator<T> Rule(Func<T, bool> predicate, string message)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (message.IsEmpty()) throw new ArgumentNullException(nameof(message), "پيام خطا نمي‌تواند خالي باشد");

            _rules.Add((predicate, message));
            return this;
        }

        /// <summary>اجرای همه‌ی قوانین ثبت‌شده و برگرداندن نتیجه‌ی کامل (شامل تمام خطاها، نه فقط اولین مورد).</summary>
        public ValidationResult Validate()
        {
            var errors = new List<string>();
            foreach (var (predicate, message) in _rules)
            {
                if (!predicate(_instance))
                    errors.Add(message);
            }

            return new ValidationResult(errors);
        }
    }
}
