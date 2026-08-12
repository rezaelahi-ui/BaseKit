using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BaseKit.Models;

namespace BaseKit.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// اگر روی مقدار Enum اتریبیوت [Description] گذاشته شده باشد آن را برمی‌گرداند،
        /// در غیر این صورت نام خود مقدار را.
        /// </summary>
        public static string Humanize(this Enum value)
        {
            var type = value.GetType();
            var field = type.GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : value.ToString();
        }

        public static int ToInt(this Enum value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Convert.ToInt32(value);
        }

        public static List<string> GetAllNames(this Enum @enum)
        {
            var result = new List<string>();
            var type = @enum.GetType();
            var values = Enum.GetValues(type);

            foreach (Enum value in values)
            {
                result.Add(value.Humanize());
            }

            return result;
        }

        public static List<T> GetAll<T>(this Enum enumValue)
        {
            var type = enumValue.GetType();

            return Enum.GetValues(type)
                       .Cast<T>()
                       .ToList();
        }

        public static List<EnumDetail> GetDetails(this Enum enumValue, bool withAll = false)
        {
            var enumType = enumValue.GetType();
            if (!enumType.IsEnum)
                throw new ArgumentException("نوع وارد شده بايد Enum باشد");

            var enumValues = Enum.GetValues(enumType);
            var result = new List<EnumDetail>();

            if (withAll)
            {
                result.Add(new EnumDetail
                {
                    Name = "همه",
                    Value = -1
                });
            }

            foreach (Enum value in enumValues)
            {
                result.Add(new EnumDetail
                {
                    Name = value.Humanize(),
                    Value = value.ToInt(),
                });
            }

            return result;
        }

        /// <summary>پارس امن یک رشته به مقدار Enum؛ در صورت نامعتبر بودن FormatException می‌دهد.</summary>
        public static T ToEnum<T>(this string value) where T : struct, Enum
        {
            if (value.IsEmpty())
                throw new ArgumentNullException(nameof(value), "مقدار وارد شده خالي يا null است");

            if (!Enum.TryParse<T>(value, true, out var result) || !Enum.IsDefined(typeof(T), result))
                throw new FormatException($"مقدار {value} به {typeof(T).Name} قابل تبديل نيست");

            return result;
        }

        /// <summary>پارس امن یک عدد به مقدار Enum؛ در صورت نامعتبر بودن FormatException می‌دهد.</summary>
        public static T ToEnum<T>(this int value) where T : struct, Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
                throw new FormatException($"مقدار {value} به {typeof(T).Name} قابل تبديل نيست");

            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
