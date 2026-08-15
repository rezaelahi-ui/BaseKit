# TodoList

ایده‌ها و متدهای پیشنهادی برای توسعه‌ی آینده‌ی BaseKit. هر آیتم وقتی پیاده‌سازی شد باید طبق قوانین پروژه (`[NotNullWhen]` در صورت نیاز + تست `Theory`/`InlineData`) تکمیل بشه.

## Extension متدهای جدید

### String
- [x] `ToBool()` — تبدیل "1"/"0"، "true"/"false"، "بله"/"خیر" به bool
- [x] `ToPersianDigits()` / `ToEnglishDigits()` — تبدیل ارقام فارسی↔انگلیسی
- [x] `NormalizeArabicChars()` — تبدیل حروف عربی (ي، ك) به فارسی (ی، ک)
- [x] `Mask()` — نمایش شماره کارت/موبایل به‌صورت `0912***1234`
- [x] `Truncate(int maxLength, string suffix = "...")` — کوتاه کردن متن با حفظ کلمه کامل
- [x] `ToPersianKeyboard()` / `ToEnglishKeyboard()` — تبدیل متنی که با چیدمان اشتباه کیبورد تایپ شده (مثلاً نیت فارسی ولی با کیبورد انگلیسی تایپ شده) به زبان درست؛ مشکل رایج فرم‌های فارسی

### Numeric
- [x] `ToPersianCurrency()` / `ToSeparatedString()` — فرمت `1,234,567`
- [x] `ToPersianWords()` — تبدیل عدد به حروف فارسی (برای چک/فاکتور)
- [x] `ToPersianOrdinalWords()` — تبدیل عدد به حروف ترتیبی فارسی («اول»، «دوم»، «سوم»، ...)، مکمل `ToPersianWords()`

### Comparable
- [x] `Between<T>(T min, T max)` روی هر `IComparable<T>` (عدد، تاریخ، رشته، `Money` و ...) — بررسی قرار گرفتن مقدار در یک بازه (شامل هر دو سر)؛ کلاس جدا (`ComparableExtensions.cs`) چون به هیچ نوع خاصی وابسته نیست

### Date
- [x] `GetPersianMonthName()` / `GetPersianDayName()` / `GetPersianSeason()` روی `DateTime` — یک پله جلوتر از `ToShamsi()` فعلی (نام ماه/روز هفته/فصل به فارسی)
- [x] `IsIranianHoliday()` — تعطیلات رسمی ایران (عید، تاسوعا/عاشورا و...)؛ چون سال به سال (به‌خصوص تعطیلات مذهبی با تقویم قمری) تغییر می‌کنه، باید لیست قابل override/تنظیم باشه، نه هاردکد ثابت
- [x] `GetShamsiYear()` / `GetShamsiMonth()` روی `DateTime` — عدد سال/ماه شمسی به‌تنهایی، بدون نیاز به parse کردن خروجی رشته‌ای `ToShamsi()`
- [x] `GetPersianDateInfo()` روی `DateTime` (ایده از یه پروژه‌ی دیگه، `DateHelper.GetDateInfo`) — مدل غنی `PersianDateInfo` شامل سال/ماه/روز/نام‌روزهفته/نام‌ماه/نام‌فصل/تعداد روزهای ماه و سال + تاریخ شروع و پایان ماه، فصل و سال جاری (هم به‌صورت رشته‌ی شمسی هم `DateTime` میلادی)
- [x] `GetWeeksOfShamsiMonth(int year, int month)` (ایده از همون پروژه، `DateHelper.GetWeeksOfMonth`) — لیست هفته‌های یک ماه شمسی (شروع هفته از شنبه)؛ هر هفته شامل شماره، تاریخ شروع/پایان (کوتاه‌شده به بازه‌ی ماه) و یک `IsComplete` که نشون می‌ده هفته تموم‌شده یا نه. به‌صورت extension روی `int year` پیاده شد (نه متد جدا)
- [x] `GetMonthsOfShamsiSeason(int year, int seasonStartMonth)` / `GetSeasonsOfShamsiYear(int year)` (همون ایده) — تفکیک یک فصل به ۳ ماه یا یک سال به ۴ فصل، هرکدوم با بازه‌ی تاریخ شروع/پایان و `IsComplete`
- [x] `ToUnixTimestamp()` روی ترکیب تاریخ‌شمسی + رشته‌ساعت (`"1402/01/01".ToUnixTimestamp("13:05")`) — وقتی تاریخ و ساعت جدا از هم ذخیره شدن و باید یکجا Unix timestamp (ms، UTC) ساخته بشه

مدل‌های جدید `PersianDateInfo`، `WeekInfo`، `MonthInfo`، `SeasonInfo` در `src/BaseKit/Models/` اضافه شدند.

### Object / Reflection
- [x] `Clone<T>()` — deep clone ساده (JSON serialize/deserialize)
- [x] `ToDictionary()` — تبدیل object به `Dictionary<string, object>` (لاگ/دیباگ)

### Exception
- [x] `GetFullMessage()` — جمع‌کردن پیام تمام InnerExceptionها در یک رشته

### Enum
- [x] `ToEnum<T>(this string)` / `ToEnum<T>(this int)` — پارس امن با پیام خطای فارسی

### Task / Async
- [x] `WithTimeout(this Task, TimeSpan)` — اجرای یک Task با timeout
- [x] `WhenAllSafe(this IEnumerable<Task>)` — منتظر همه‌ی تسک‌ها می‌مونه حتی اگه بعضی fail بشن، و همه‌ی exceptionها رو (نه فقط اولی رو، بر خلاف `Task.WhenAll`) در یک `AggregateException` جمع می‌کنه. یک overload جنریک هم برای `IEnumerable<Task<T>>` اضافه شد

### Validation
- [x] `IsValidNationalCode()` — اعتبارسنجی کد ملی ایرانی با الگوریتم چک‌دیجیت
- [x] `IsValidMobileNumber()` — شماره موبایل ایران (`09xxxxxxxxx`)
- [x] `IsValidEmail()`
- [x] `IsValidIban()` — اعتبارسنجی شماره شبا (mod-97 عمومی، برای IR هم کار می‌کند)
- [x] `IsValidPostalCode()` — کدپستی ۱۰رقمی ایران؛ چون کدپستی برخلاف کد ملی الگوریتم چک‌دیجیت رسمی ندارد، فقط فرمت (۱۰ رقم) و رد ارقام تکراری بدیهاً نامعتبر بررسی می‌شود
- [x] `IsValidLegalNationalId()` — شناسه‌ملی اشخاص حقوقی (شرکت‌ها)؛ الگوریتم چک‌دیجیتش با `IsValidNationalCode()` (اشخاص حقیقی) فرق داره
- [x] `IsValidCardNumber()` + `GetBankName()` — اعتبارسنجی شماره کارت با الگوریتم Luhn + تشخیص نام بانک از ۶ رقم اول (BIN)؛ جدول BIN به بانک‌های شناخته‌شده و پرکاربرد محدوده، نه منبع رسمی/کامل
- [x] `GetBankNameFromIban()` — تشخیص نام بانک از کد سه‌رقمی بانک داخل شماره شبا (بلافاصله بعد از `IRxx`)
- [x] `IsValidPlateNumber()` — پلاک خودرو ایران (فرمت `12ب34567`)

### Collections بیشتر
- [x] `ForEach<T>(this IEnumerable<T>, Action<T>)`
- [x] `ChunkBy<T>(this IEnumerable<T>, int size)` — عمداً هم‌نام با `Enumerable.Chunk` نت 6+ نیست (جلوگیری از Ambiguous call)
- [x] `DistinctByKey<T,TKey>()` — عمداً هم‌نام با `Enumerable.DistinctBy` نت 6+ نیست
- [x] `Page(int pageNumber, int pageSize)` — صفحه‌بندی لیست
- [x] `Shuffle<T>(this IList<T>)` — به‌هم‌ریختن ترتیب لیست (Fisher-Yates)
- [x] `RandomItem<T>(this IEnumerable<T>)` — انتخاب تصادفی یک آیتم از لیست

### Retry / Resilience
- [x] `RetryAsync(this Func<Task>, int retryCount, TimeSpan? delay)` — تلاش مجدد ساده بدون نیاز به Polly
- [x] `RetryWithBackoffAsync(...)` با **exponential backoff + jitter** — فاصله‌ی بین تلاش‌ها به‌جای ثابت‌بودن نمایی افزایش پیدا می‌کنه (با کمی نویز رندوم)، مناسب صدا زدن APIهای بیرونی. به‌جای overload اسم جدا گرفت تا با overloadهای مبتنی‌بر پارامتر پیش‌فرض `RetryAsync` تداخل نکنه

### Caching سبک
- [x] `SimpleCache<TKey,TValue>` با expiration ساده (in-memory، نه Redis)

### Logging / Debug کمکی
- [x] `Dump()` — پرینت خوانا از هر object (JSON indented) برای دیباگ سریع
- [x] `ToJson()` / `FromJson<T>()` — wrapper کوتاه روی `System.Text.Json`
- [x] `Measure()` روی `Action`/`Func<T>` — زمان اجرا رو اندازه می‌گیره و برمی‌گردونه، برای پروفایلینگ سریع بدون `Stopwatch` دستی

### File / Path
- [x] `EnsureDirectoryExists()` — روی مسیر رشته‌ای (نسخه‌ی `FileInfo` فعلاً اضافه نشده)
- [x] `GetSafeFileName()` — حذف کاراکترهای غیرمجاز از نام فایل

### Fuzzy Matching (شباهت متن)
- [x] `LevenshteinDistance(this string, string)` — تعداد کمینه‌ی عملیات ویرایشی (insert/delete/replace) برای تبدیل یک رشته به دیگری
- [x] `SimilarityTo(this string, string)` — درصد شباهت بین ۰ و ۱، محاسبه‌شده از `LevenshteinDistance` (`1 - distance / max(len1, len2)`)
- [x] `IsSimilarTo(this string, string, double threshold = 0.8)` — true اگر شباهت بیشتر یا مساوی `threshold` باشد
- [x] `FindBestMatch(this IEnumerable<string>, string query)` — نزدیک‌ترین آیتم لیست به query (بیشترین شباهت)
- [x] `FindSimilar(this IEnumerable<string>, string query, double threshold = 0.8)` — همه‌ی آیتم‌های لیست که شباهتشان به query حداقل `threshold` است، مرتب‌شده بر اساس شباهت نزولی؛ خروجی `IEnumerable<(string Item, double Score)>`
  - مثال: `cities.FindSimilar("خوراسان جنوبی", 0.8)` → شامل `("خراسان جنوبی", 0.923)`

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
- [x] **`Option<T>`** (`Common/Option.cs`) — مکمل `Result<T>` فعلی؛ برای جایی که فقط «هست/نیست» مهمه، نه پیام خطای مشخص. شامل `Some`/`None`/`FromNullable`/`GetValueOrThrow`/`GetValueOrDefault`/`TryGetValue`/`Match`

## Attributes (پورت‌شده از پروژه‌های اصلی)

- [x] **`PersianRegularExpressionAttribute`** (از `AtiranRegularExpression`) — اعتبارسنجی Regex با پیام فارسی؛ برای پراپرتی nullable مقدار خالی را معتبر می‌داند (تشخیص `string?` فقط در .NET 6+ با `NullabilityInfoContext`، برای همین پروژه الان **multi-target شد**: `netstandard2.0;net6.0`)
- [x] **`GreaterThanAttribute`** — بزرگ‌تر یا مساوی یک آستانه؛ مقدار null نامعتبر تلقی نمی‌شود (الزامی‌بودن مسئولیت `[Required]` است)
- [x] **`PersianRangeAttribute`** — بازه‌ی [min, max] با پیام فارسی
- [x] **`PersianRequiredAttribute`** — نسخه‌ی فارسی `RequiredAttribute`، رشته‌ی whitespace را هم نامعتبر می‌داند
- [x] **`NoteAttribute`** — متادیتای مستندسازی روی متد (Summary/Description)، نه اعتبارسنجی

هر چهار اتریبیوت اعتبارسنجی طبق pattern استاندارد `ValidationAttribute` بازنویسی شدند: به‌جای throw کردن exception، `ValidationResult` ناموفق (یا `false` در overload قدیمی) برمی‌گردانند — سازگار با `Validator.TryValidateObject` و جمع‌آوری همه‌ی خطاها، نه فقط اولین مورد. `BadRequestException` (در `BaseKit.Exceptions`) به‌عنوان یک exception عمومی برای لایه‌ی API نگه داشته شد، ولی دیگر داخل این attributeها پرتاب نمی‌شود.

### Attributes بیشتر

اعتبارسنجی‌های موجود در `ValidationExtensions` به‌شکل Attribute هم دراومدن تا مستقیم روی پراپرتی مدل (خصوصاً برای ASP.NET/EF Core) قابل استفاده باشند:
- [x] `PersianNationalCodeAttribute` — از `IsValidNationalCode()`
- [x] `PersianMobileNumberAttribute` — از `IsValidMobileNumber()`
- [x] `IranianIbanAttribute` — از `IsValidIban()`

مقایسه‌ی بین دو فیلد (cross-property)، چیزی که DataAnnotations استاندارد پوشش نمی‌دهد:
- [x] `CompareToAttribute` — رابطه‌ی `<`/`<=`/`>`/`>=`/`==`/`!=` بین یک فیلد و فیلد دیگر مدل (`CompareType` enum)؛ BCL فقط `CompareAttribute` برای equality دارد
- [x] `DateRangeAttribute` — بررسی این‌که تاریخ شروع قبل از (یا مساوی) پایان باشد؛ روی `DateTime` و رشته‌های تاریخ (مثل فرمت شمسی) کار می‌کند

اعتبارسنجی شرطی:
- [x] `RequiredIfAttribute` — یک فیلد فقط وقتی الزامی است که فیلد دیگر مدل مقدار خاصی داشته باشد

فایل/آپلود (بدون وابستگی مستقیم به ASP.NET؛ duck-typed روی پراپرتی‌های `FileName`/`Length`):
- [x] `AllowedExtensionsAttribute`
- [x] `MaxFileSizeAttribute`

متادیتا (نه اعتبارسنجی، شبیه `NoteAttribute`):
- [x] `DisplayOrderAttribute`
- [x] `AuditIgnoreAttribute`

همه‌ی attributeهای اعتبارسنجی از `ValidationContextHelpers.GetMemberNames` مشترک استفاده می‌کنن (کد تکراری بین `GreaterThanAttribute`/`PersianRegularExpressionAttribute` هم جمع شد). همه با null-safe بودن ساخته شدن: مقدار null همیشه معتبره مگر `RequiredIfAttribute` (که دقیقاً برای همین ساخته شده) — الزامی‌بودن مسئولیت `[Required]`/`PersianRequiredAttribute`ه.
