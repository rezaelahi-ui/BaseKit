# TodoList

ایده‌ها و متدهای پیشنهادی برای توسعه‌ی آینده‌ی BaseKit. هر آیتم وقتی پیاده‌سازی شد باید طبق قوانین پروژه (`[NotNullWhen]` در صورت نیاز + تست `Theory`/`InlineData`) تکمیل بشه.

## Extension متدهای جدید

### String
- [x] `ToBool()` — تبدیل "1"/"0"، "true"/"false"، "بله"/"خیر" به bool
- [x] `ToPersianDigits()` / `ToEnglishDigits()` — تبدیل ارقام فارسی↔انگلیسی
- [x] `NormalizeArabicChars()` — تبدیل حروف عربی (ي، ك) به فارسی (ی، ک)
- [x] `Mask()` — نمایش شماره کارت/موبایل به‌صورت `0912***1234`
- [x] `Truncate(int maxLength, string suffix = "...")` — کوتاه کردن متن با حفظ کلمه کامل

### Numeric
- [x] `ToPersianCurrency()` / `ToSeparatedString()` — فرمت `1,234,567`
- [x] `ToPersianWords()` — تبدیل عدد به حروف فارسی (برای چک/فاکتور)

### Object / Reflection
- [x] `Clone<T>()` — deep clone ساده (JSON serialize/deserialize)
- [x] `ToDictionary()` — تبدیل object به `Dictionary<string, object>` (لاگ/دیباگ)

### Exception
- [x] `GetFullMessage()` — جمع‌کردن پیام تمام InnerExceptionها در یک رشته

### Enum
- [x] `ToEnum<T>(this string)` / `ToEnum<T>(this int)` — پارس امن با پیام خطای فارسی

### Task / Async
- [x] `WithTimeout(this Task, TimeSpan)` — اجرای یک Task با timeout

### Validation
- [x] `IsValidNationalCode()` — اعتبارسنجی کد ملی ایرانی با الگوریتم چک‌دیجیت
- [x] `IsValidMobileNumber()` — شماره موبایل ایران (`09xxxxxxxxx`)
- [x] `IsValidEmail()`
- [x] `IsValidIban()` — اعتبارسنجی شماره شبا (mod-97 عمومی، برای IR هم کار می‌کند)

### Collections بیشتر
- [x] `ForEach<T>(this IEnumerable<T>, Action<T>)`
- [x] `ChunkBy<T>(this IEnumerable<T>, int size)` — عمداً هم‌نام با `Enumerable.Chunk` نت 6+ نیست (جلوگیری از Ambiguous call)
- [x] `DistinctByKey<T,TKey>()` — عمداً هم‌نام با `Enumerable.DistinctBy` نت 6+ نیست
- [x] `Page(int pageNumber, int pageSize)` — صفحه‌بندی لیست

### Retry / Resilience
- [x] `RetryAsync(this Func<Task>, int retryCount, TimeSpan? delay)` — تلاش مجدد ساده بدون نیاز به Polly

### Caching سبک
- [x] `SimpleCache<TKey,TValue>` با expiration ساده (in-memory، نه Redis)

### Logging / Debug کمکی
- [x] `Dump()` — پرینت خوانا از هر object (JSON indented) برای دیباگ سریع
- [x] `ToJson()` / `FromJson<T>()` — wrapper کوتاه روی `System.Text.Json`

### File / Path
- [x] `EnsureDirectoryExists()` — روی مسیر رشته‌ای (نسخه‌ی `FileInfo` فعلاً اضافه نشده)
- [x] `GetSafeFileName()` — حذف کاراکترهای غیرمجاز از نام فایل

### پکیج‌های جداگانه (وابسته به فریم‌ورک‌های خارجی، نباید در core باشن)
- [ ] `BaseKit.Extensions.Configuration` — مثل `GetOrDefault<T>(this IConfiguration, string key, T defaultValue)` (وابسته به `Microsoft.Extensions.Configuration.Abstractions`)

## زیرساختی

- [x] کلاس **Guard clauses** (`Guard.Against.Null(...)`, `Guard.Against.Empty(...)`) به‌جای تکرار `if (x.IsEmpty()) throw ...`
- [x] الگوی **Result&lt;T&gt;** ساده برای برگردوندن نتیجه بدون exception (جایگزین بعضی throwها در متدهای `To*`)
- [x] مدل **`PagedResult<T>`** برای برگردوندن یک صفحه از نتایج در یک پاسخ استاندارد، شامل:
  - `Items` — لیست مقادیر همان صفحه
  - `PageNumber`, `PageSize`
  - `TotalCount` — تعداد کل رکوردها (نه فقط صفحه فعلی)
  - `TotalPages` — محاسبه‌شده از `TotalCount`/`PageSize`
  - `HasNextPage`, `HasPreviousPage`
  - متد کمکی `ToPagedResult<T>(this IEnumerable<T>, int pageNumber, int pageSize)` که از `Page()` فعلی در `EnumerableExtensions` استفاده می‌کنه و این مدل رو می‌سازه
- [x] **`Money`** — value object برای مبلغ + واحد پول (`Common/Money.cs`)؛ جمع/تفریق/مقایسه با واحد متفاوت `InvalidOperationException` می‌دهد، `ToMoney(this decimal, string currency)` هم در `NumericExtensions` اضافه شد
- [x] **Business-day helpers** روی `DateExtensions`: `IsWeekend()` (پنج‌شنبه+جمعه، قابل تنظیم)، `NextWorkingDay()`، `AddWorkingDays()`
- [x] **Fluent Validator** (`Common/Validator.cs` + `Common/ValidationResult.cs`) — بر خلاف `Guard` که در اولین خطا throw می‌کند، همه‌ی قوانین را چک کرده و لیست کامل خطاها را برمی‌گرداند
