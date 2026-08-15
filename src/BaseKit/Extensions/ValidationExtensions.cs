using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای اعتبارسنجی رشته‌های رایج ایرانی/عمومی (کد ملی، موبایل، ایمیل، شبا).</summary>
    public static class ValidationExtensions
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private static readonly Regex MobileRegex =
            new(@"^(?:\+98|0098|0)?9\d{9}$", RegexOptions.Compiled);

        private static readonly Regex IbanFormatRegex =
            new(@"^[A-Z]{2}\d{2}[A-Z0-9]+$", RegexOptions.Compiled);

        private static readonly Regex PlateNumberRegex =
            new(@"^\d{2}[ابپتثجدزسشصطعفقکگلمنوهی]\d{5}$", RegexOptions.Compiled);

        /// <summary>اعتبارسنجی کد ملی ایرانی با الگوریتم چک‌دیجیت.</summary>
        public static bool IsValidNationalCode([NotNullWhen(true)] this string? code)
        {
            if (code.IsEmpty()) return false;

            code = code.ToEnglishDigits().Trim();
            if (code.Length != 10 || !Regex.IsMatch(code, @"^\d{10}$")) return false;

            // کد ملی‌هایی با ۱۰ رقم یکسان (مثل 0000000000) نامعتبرند
            var allSameDigits = true;
            for (var i = 1; i < code.Length; i++)
            {
                if (code[i] != code[0]) { allSameDigits = false; break; }
            }
            if (allSameDigits) return false;

            var checkDigit = code[9] - '0';
            var sum = 0;
            for (var i = 0; i < 9; i++)
                sum += (code[i] - '0') * (10 - i);

            var remainder = sum % 11;
            return remainder < 2 ? checkDigit == remainder : checkDigit == 11 - remainder;
        }

        /// <summary>اعتبارسنجی شماره موبایل ایران (با یا بدون پیش‌شماره 0 / +98 / 0098).</summary>
        public static bool IsValidMobileNumber([NotNullWhen(true)] this string? number)
        {
            if (number.IsEmpty()) return false;

            var normalized = number.ToEnglishDigits().Trim();
            return MobileRegex.IsMatch(normalized);
        }

        /// <summary>اعتبارسنجی ساده‌ی فرمت ایمیل (نه بررسی وجود واقعی دامنه/mailbox).</summary>
        public static bool IsValidEmail([NotNullWhen(true)] this string? email)
        {
            if (email.IsEmpty()) return false;

            return EmailRegex.IsMatch(email.Trim());
        }

        /// <summary>
        /// اعتبارسنجی شماره شبا/IBAN با الگوریتم استاندارد mod-97 (ISO 7064).
        /// برای شبای ایران با پیشوند IR و برای IBAN سایر کشورها هم کار می‌کند.
        /// </summary>
        public static bool IsValidIban([NotNullWhen(true)] this string? iban)
        {
            if (iban.IsEmpty()) return false;

            var normalized = iban.Replace(" ", "").ToUpperInvariant();
            if (!IbanFormatRegex.IsMatch(normalized)) return false;

            var rearranged = normalized.Substring(4) + normalized.Substring(0, 4);

            var remainder = 0;
            foreach (var c in rearranged)
            {
                var digitValue = char.IsLetter(c) ? c - 'A' + 10 : c - '0';
                foreach (var digitChar in digitValue.ToString())
                    remainder = (remainder * 10 + (digitChar - '0')) % 97;
            }

            return remainder == 1;
        }

        /// <summary>
        /// اعتبارسنجی فرمت کدپستی ۱۰رقمی ایران. برخلاف کد ملی، کدپستی الگوریتم چک‌دیجیت رسمی ندارد؛
        /// این متد فقط فرمت (۱۰ رقم) و رد کدهای بدیهاً نامعتبر (مثل ۱۰ رقم یکسان) را بررسی می‌کند،
        /// نه صحت واقعی/وجود کد نزد شرکت پست.
        /// </summary>
        public static bool IsValidPostalCode([NotNullWhen(true)] this string? postalCode)
        {
            if (postalCode.IsEmpty()) return false;

            var normalized = postalCode.ToEnglishDigits().Replace(" ", "").Replace("-", "").Trim();
            if (!Regex.IsMatch(normalized, @"^\d{10}$")) return false;

            var allSameDigits = true;
            for (var i = 1; i < normalized.Length; i++)
            {
                if (normalized[i] != normalized[0]) { allSameDigits = false; break; }
            }

            return !allSameDigits;
        }

        // ضرایب الگوریتم چک‌دیجیت شناسه‌ملی اشخاص حقوقی (شرکت‌ها)؛ با الگوریتم کد ملی اشخاص حقیقی متفاوت است.
        private static readonly int[] LegalNationalIdCoefficients = { 29, 27, 23, 19, 17, 29, 27, 23, 19, 17 };

        /// <summary>اعتبارسنجی شناسه‌ملی ۱۱رقمی اشخاص حقوقی (شرکت‌ها) با الگوریتم چک‌دیجیت مخصوص آن؛ با <see cref="IsValidNationalCode"/> (اشخاص حقیقی) اشتباه گرفته نشود.</summary>
        public static bool IsValidLegalNationalId([NotNullWhen(true)] this string? legalNationalId)
        {
            if (legalNationalId.IsEmpty()) return false;

            var normalized = legalNationalId.ToEnglishDigits().Trim();
            if (!Regex.IsMatch(normalized, @"^\d{11}$")) return false;

            var checkDigit = normalized[10] - '0';
            var sum = 0;
            for (var i = 0; i < 10; i++)
                sum += (normalized[i] - '0') * LegalNationalIdCoefficients[i];

            var remainder = sum % 11;
            return remainder < 2 ? checkDigit == remainder : checkDigit == 11 - remainder;
        }

        /// <summary>اعتبارسنجی شماره کارت بانکی ۱۶رقمی با الگوریتم Luhn (بدون بررسی وجود واقعی کارت نزد بانک).</summary>
        public static bool IsValidCardNumber([NotNullWhen(true)] this string? cardNumber)
        {
            if (cardNumber.IsEmpty()) return false;

            var normalized = cardNumber.ToEnglishDigits().Replace(" ", "").Replace("-", "").Trim();
            if (!Regex.IsMatch(normalized, @"^\d{16}$")) return false;

            var sum = 0;
            var doubleDigit = false;
            for (var i = normalized.Length - 1; i >= 0; i--)
            {
                var digit = normalized[i] - '0';
                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return sum % 10 == 0;
        }

        // نگاشت ۶ رقم اول (BIN) شماره کارت به نام بانک؛ فهرست بانک‌های شناخته‌شده و پرکاربرد است، نه یک منبع رسمی/کامل،
        // و ممکن است با ادغام/تغییر بانک‌ها به‌مرور قدیمی شود.
        private static readonly Dictionary<string, string> CardBinToBankName = new()
        {
            ["603799"] = "بانک ملی ایران",
            ["589210"] = "بانک سپه",
            ["627353"] = "بانک تجارت",
            ["585983"] = "بانک تجارت",
            ["603769"] = "بانک صادرات ایران",
            ["603770"] = "بانک کشاورزی",
            ["610433"] = "بانک ملت",
            ["622106"] = "بانک پارسیان",
            ["639194"] = "بانک پارسیان",
            ["502229"] = "بانک پاسارگاد",
            ["639347"] = "بانک پاسارگاد",
            ["621986"] = "بانک سامان",
            ["627760"] = "پست بانک ایران",
            ["628023"] = "بانک مسکن",
            ["627412"] = "بانک اقتصاد نوین",
        };

        /// <summary>
        /// تشخیص نام بانک از ۶ رقم اول (BIN) شماره کارت بانکی ایرانی؛ اگر BIN شناخته‌شده نباشد null برمی‌گرداند.
        /// فهرست، بانک‌های شناخته‌شده و پرکاربرد را پوشش می‌دهد، نه لزوماً همه‌ی بانک‌ها را.
        /// </summary>
        public static string? GetBankName(this string? cardNumber)
        {
            var normalized = cardNumber?.ToEnglishDigits().Replace(" ", "").Replace("-", "").Trim();
            if (normalized is null || normalized.Length < 6) return null;

            return CardBinToBankName.TryGetValue(normalized.Substring(0, 6), out var bankName) ? bankName : null;
        }

        // نگاشت کد سه‌رقمی بانک در شبای ایران (بلافاصله بعد از IRxx) به نام بانک؛ همان محدودیت CardBinToBankName را دارد.
        private static readonly Dictionary<string, string> ShebaBankCodeToBankName = new()
        {
            ["011"] = "بانک صنعت و معدن",
            ["012"] = "بانک ملت",
            ["016"] = "بانک کشاورزی",
            ["017"] = "بانک ملی ایران",
            ["018"] = "بانک تجارت",
            ["019"] = "بانک صادرات ایران",
            ["021"] = "پست بانک ایران",
            ["054"] = "بانک پارسیان",
            ["056"] = "بانک سامان",
            ["057"] = "بانک پاسارگاد",
        };

        /// <summary>
        /// تشخیص نام بانک از کد سه‌رقمی بانک داخل شماره شبای ایران (بلافاصله بعد از IRxx)؛
        /// اگر فرمت شبا نامعتبر باشد یا کد بانک شناخته‌شده نباشد null برمی‌گرداند.
        /// </summary>
        public static string? GetBankNameFromIban(this string? iban)
        {
            if (iban.IsEmpty()) return null;

            var normalized = iban.Replace(" ", "").ToUpperInvariant();
            if (!normalized.StartsWith("IR") || normalized.Length < 7) return null;

            var bankCode = normalized.Substring(4, 3);
            return ShebaBankCodeToBankName.TryGetValue(bankCode, out var bankName) ? bankName : null;
        }

        /// <summary>اعتبارسنجی فرمت پلاک خودروی ایران (مثل «۱۲ب۳۴۵۶۷»، بدون کد دورقمی «ایران»).</summary>
        public static bool IsValidPlateNumber([NotNullWhen(true)] this string? plateNumber)
        {
            if (plateNumber.IsEmpty()) return false;

            var normalized = plateNumber.ToEnglishDigits().NormalizeArabicChars().Replace(" ", "").Trim();
            return PlateNumberRegex.IsMatch(normalized);
        }
    }
}
