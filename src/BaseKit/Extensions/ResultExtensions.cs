using System;
using BaseKit.Common;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای پل زدن بین دنیای exception-based و <see cref="Result{T}"/>.</summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// اجرای <paramref name="func"/> و برگرداندن نتیجه در قالب <see cref="Result{T}"/>: موفق با مقدار برگشتی،
        /// یا ناموفق با پیام exception در صورت پرتاب؛ جایگزین try/catch دستی برای تبدیل کد throw-based به Result.
        /// </summary>
        public static Result<T> ToResult<T>(this Func<T> func)
        {
            if (func is null) throw new ArgumentNullException(nameof(func));

            try
            {
                return Result<T>.Success(func());
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(ex.Message);
            }
        }
    }
}
