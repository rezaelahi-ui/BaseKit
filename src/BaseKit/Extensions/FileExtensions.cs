using System;
using System.IO;
using System.Linq;

namespace BaseKit.Extensions
{
    public static class FileExtensions
    {
        /// <summary>اگر پوشه‌ی مسیر داده‌شده وجود نداشته باشد آن را می‌سازد؛ خود مسیر را برمی‌گرداند.</summary>
        public static string EnsureDirectoryExists(this string path)
        {
            if (path.IsEmpty()) throw new ArgumentNullException(nameof(path), "مقدار وارد شده خالي يا null است");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>حذف کاراکترهای غیرمجاز نام فایل (مثل / \ : * ? " &lt; &gt; |) از رشته‌ی ورودی.</summary>
        public static string GetSafeFileName(this string fileName)
        {
            if (fileName.IsEmpty()) throw new ArgumentNullException(nameof(fileName), "مقدار وارد شده خالي يا null است");

            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());
        }
    }
}
