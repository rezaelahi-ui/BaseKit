using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace BaseKit.Extensions
{
    public static class ValidationExtensions
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private static readonly Regex MobileRegex =
            new(@"^(?:\+98|0098|0)?9\d{9}$", RegexOptions.Compiled);

        private static readonly Regex IbanFormatRegex =
            new(@"^[A-Z]{2}\d{2}[A-Z0-9]+$", RegexOptions.Compiled);

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
    }
}
