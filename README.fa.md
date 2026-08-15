# BaseKit

🇬🇧 [Read in English](README.md)

مجموعه‌ای از Extension methodها، Attributeها و ابزارهای کمکی که برای استفاده‌ی مشترک بین پروژه‌های مختلف (از .NET Framework 4.6.1 تا .NET 10) ساخته شده. تمرکز اصلی روی نیازهای پروژه‌های فارسی/ایرانی (تاریخ شمسی، اعتبارسنجی کد ملی/موبایل/شبا، اعداد فارسی) به‌همراه ابزارهای عمومی (Guard clauses، Result pattern، صفحه‌بندی، کش ساده و ...).

## نصب

```bash
dotnet add package BaseKit
```

Target frameworks: `netstandard2.0` (سازگار با .NET Framework 4.6.1+ و .NET Core/5+) و `net6.0` (برای قابلیت‌هایی مثل تشخیص nullable بودن reference typeها که فقط در .NET 6+ در دسترسن؛ پروژه‌های .NET 7 تا 10 هم به‌صورت خودکار از build مخصوص net6.0 استفاده می‌کنن).

---

## فهرست

- [String Extensions](#string-extensions)
- [Numeric Extensions](#numeric-extensions)
- [Comparable Extensions](#comparable-extensions)
- [Date Extensions (تاریخ شمسی)](#date-extensions-تاریخ-شمسی)
- [Validation Extensions](#validation-extensions)
- [Fuzzy Matching (شباهت متن)](#fuzzy-matching-شباهت-متن)
- [Enum Extensions](#enum-extensions)
- [Collection Extensions](#collection-extensions)
- [Object / Reflection Extensions](#object--reflection-extensions)
- [Exception Extensions](#exception-extensions)
- [Task Extensions](#task-extensions)
- [File Extensions](#file-extensions)
- [Debug / Logging Extensions](#debug--logging-extensions)
- [IP Extensions](#ip-extensions)
- [Common: Money](#common-money)
- [Common: Result\<T\>](#common-resultt)
- [Common: Option\<T\>](#common-optiont)
- [Common: PagedResult\<T\>](#common-pagedresultt)
- [Common: Validator (Fluent)](#common-validator-fluent)
- [Common: SimpleCache](#common-simplecache)
- [Guard Clauses](#guard-clauses)
- [Data Annotation Attributes](#data-annotation-attributes)
- [Exceptions](#exceptions)

---

### String Extensions

```csharp
"".IsEmpty();                          // true — null/خالی/فقط‌whitespace
"value".IsNotEmpty();                  // true

"1,234".ToInt();                       // 1234 (پشتیبانی از جداکننده‌ی کاما)
"1,234.5".ToDecimal();                 // 1234.5m
"1,234.5".ToDouble();                  // 1234.5
"123456789012".ToLong();               // 123456789012
"192.168.1.1".ToIp();                  // IPAddress
"https://example.com".ToUri();         // Uri (باید با http/https شروع بشه)
"1".ToBool();                          // true (1/0، true/false، yes/no، بله/خیر)

"123".ToPersianDigits();               // "۱۲۳"
"۱۲۳".ToEnglishDigits();                // "123" (فارسی و عربی هر دو پشتیبانی می‌شن)
"كتاب".NormalizeArabicChars();          // "کتاب" (ي/ك عربی → ی/ک فارسی)

"09123456789".Mask();                  // "0912***6789"
"یک متن طولانی است".Truncate(10);       // با حفظ کلمه‌ی کامل + "..."

"hgt".ToPersianKeyboard();             // "الف" (اصلاح متنی که با چیدمان اشتباه کیبورد تایپ شده)
"الف".ToEnglishKeyboard();             // "hgt"
```

### Numeric Extensions

```csharp
1234567.ToSeparatedString();           // "1,234,567"
1234567L.ToPersianCurrency();          // "۱,۲۳۴,۵۶۷ ریال"
1234567L.ToPersianWords();             // "یک میلیون و دویست و سی و چهار هزار و پانصد و شصت و هفت"
21L.ToPersianOrdinalWords();           // "بیست و یکم"
500m.ToMoney("IRR");                   // Money
```

### Comparable Extensions

```csharp
5.Between(1, 10);                      // true — هر دو سر بازه شامل می‌شن
DateTime.Today.Between(rangeStart, rangeEnd);
"m".Between("a", "z");                 // روی هر IComparable<T> کار می‌کنه (عدد، تاریخ، رشته، Money و ...)
```

### Date Extensions (تاریخ شمسی)

```csharp
DateTime.Now.ToShamsi();               // "1402/01/01"
DateTime.Now.ToClock();                // "13:05:09"
"1402/01/01".ToGregorian();            // DateTime
"1402/01/01".IsValidShamsiDate();      // true
"1402/02/01".IsGreaterThan("1402/01/01"); // مقایسه‌ی رشته‌ای تاریخ‌ها

// روزهای کاری (پنج‌شنبه + جمعه به‌عنوان تعطیل، قابل تنظیم)
DateTime.Today.IsWeekend();
DateTime.Today.NextWorkingDay();
DateTime.Today.AddWorkingDays(5);

DateTime.Now.GetPersianMonthName();    // "فروردین"
DateTime.Now.GetPersianDayName();      // "شنبه"
DateTime.Now.GetPersianSeason();       // "بهار"
DateTime.Now.GetShamsiYear();          // 1402 (به‌صورت عدد، بدون نیاز به parse رشته)
DateTime.Now.GetShamsiMonth();         // 1

// تعطیلات رسمی ایران: آخر هفته + تعطیلات ثابت شمسی (نوروز و ...).
// تعطیلات مذهبی/قمری چون سال به سال جابه‌جا می‌شن، باید هرساله جداگانه پاس داده بشن.
DateTime.Today.IsIranianHoliday();
DateTime.Today.IsIranianHoliday(extraHolidaysShamsi: new[] { "1402/06/06" }); // مثلاً عاشورا در همون سال

// اطلاعات کامل ماه/فصل/سالی که یک تاریخ در آن قرار دارد (PersianDateInfo)
PersianDateInfo info = DateTime.Now.GetPersianDateInfo();
// info.Year، info.MonthName، info.SeasonName، info.DaysInMonth،
// info.MonthStartShamsi/MonthEndShamsi، info.SeasonStartDate/SeasonEndDate، ...

List<WeekInfo> weeks = 1402.GetWeeksOfShamsiMonth(month: 1);     // هفته‌های فروردین ۱۴۰۲ (شروع از شنبه)
List<MonthInfo> months = 1402.GetMonthsOfShamsiSeason(seasonStartMonth: 1); // سه ماه بهار ۱۴۰۲
List<SeasonInfo> seasons = 1402.GetSeasonsOfShamsiYear();        // چهار فصل سال ۱۴۰۲

"1402/01/01".ToUnixTimestamp("13:05:09"); // ترکیب تاریخ شمسی + رشته‌ی ساعت به Unix timestamp (میلی‌ثانیه، UTC)
```

### Validation Extensions

```csharp
"0499370899".IsValidNationalCode();     // اعتبارسنجی کد ملی با الگوریتم چک‌دیجیت
"09123456789".IsValidMobileNumber();    // شماره موبایل ایران
"test@example.com".IsValidEmail();
"DE89370400440532013000".IsValidIban(); // شبا/IBAN با الگوریتم استاندارد mod-97

"1234567890".IsValidPostalCode();       // کدپستی ۱۰رقمی ایران (فقط فرمت؛ الگوریتم چک‌دیجیت رسمی‌ای وجود ندارد)
"12345678918".IsValidLegalNationalId(); // شناسه‌ملی اشخاص حقوقی (شرکت) — الگوریتم چک‌دیجیتش با کد ملی اشخاص حقیقی فرق داره

"6037-9900-0000-0006".IsValidCardNumber(); // شماره کارت ۱۶رقمی، الگوریتم Luhn
"6037990000000006".GetBankName();          // "بانک ملی ایران" (از روی BIN؛ فقط بانک‌های شناخته‌شده، برای ناشناخته null برمی‌گرداند)
"IR120170000000000000000000".GetBankNameFromIban(); // نام بانک از روی کد سه‌رقمی بانک داخل شبا

"12ب34567".IsValidPlateNumber();        // فرمت پلاک خودروی ایران
```

### Fuzzy Matching (شباهت متن)

```csharp
"خراسان جنوبی".LevenshteinDistance("خوراسان جنوبی"); // 1
"خراسان جنوبی".SimilarityTo("خوراسان جنوبی");         // 0.923
"خراسان جنوبی".IsSimilarTo("خوراسان جنوبی", 0.8);      // true

var cities = new[] { "خراسان جنوبی", "خراسان رضوی", "تهران" };
cities.FindBestMatch("خوراسان جنوبی");                  // "خراسان جنوبی"
cities.FindSimilar("خوراسان جنوبی", threshold: 0.8);    // [("خراسان جنوبی", 0.923)]
```

### Enum Extensions

```csharp
MyEnum.Value.Humanize();               // از [Description] یا نام enum
MyEnum.Value.ToInt();
MyEnum.Value.GetAllNames();
MyEnum.Value.GetDetails(withAll: true); // List<EnumDetail> برای dropdown
"Value".ToEnum<MyEnum>();              // پارس امن با پیام خطای فارسی
2.ToEnum<MyEnum>();
```

### Collection Extensions

```csharp
list.IsEmpty();
list.ForEach(x => Console.WriteLine(x));
items.ChunkBy(3);                      // تقسیم به دسته‌های ۳تایی (هم‌نام با Chunk نت 6+ نیست، بدون تداخل)
items.DistinctByKey(x => x.Id);        // (هم‌نام با DistinctBy نت 6+ نیست)
items.Page(pageNumber: 2, pageSize: 20);
items.ToPagedResult(pageNumber: 2, pageSize: 20); // PagedResult<T> با TotalPages/HasNextPage/...
oldList.HasChanges(newList);

list.Shuffle();                        // به‌هم‌ریختن لیست به‌صورت درجا (Fisher-Yates)
items.RandomItem();                    // یک آیتم تصادفی از دنباله
```

### Object / Reflection Extensions

```csharp
var clone = myObject.Clone();          // deep clone با JSON serialize/deserialize
var dict = myObject.ToDictionary();    // Dictionary<string, object?> از پراپرتی‌های public
```

### Exception Extensions

```csharp
exception.GetFullMessage();            // پیام کامل شامل همه‌ی InnerExceptionها
```

### Task Extensions

```csharp
await someTask.WithTimeout(TimeSpan.FromSeconds(5));   // TimeoutException در صورت تایم‌اوت

Func<Task<int>> operation = () => CallExternalServiceAsync();
await operation.RetryAsync(retryCount: 3, delay: TimeSpan.FromSeconds(1));
await operation.RetryWithBackoffAsync(retryCount: 5); // فاصله‌ی نمایی (exponential backoff) + jitter بین تلاش‌ها

// منتظر همه‌ی Taskها می‌مونه حتی اگه بعضی fail بشن، و همه‌ی exceptionهاشون رو جمع می‌کنه
// (بر خلاف Task.WhenAll که فقط اولی رو نشون می‌ده)
await tasks.WhenAllSafe();
```

### File Extensions

```csharp
@"C:\logs\app".EnsureDirectoryExists();
"report:2024/06.pdf".GetSafeFileName(); // حذف کاراکترهای غیرمجاز
```

### Debug / Logging Extensions

```csharp
myObject.Dump();                       // JSON خوانا برای دیباگ سریع
myObject.ToJson();
json.FromJson<MyDto>();

Action action = () => DoSomeWork();
TimeSpan elapsed = action.Measure();                      // Action ← فقط زمان اجرا

Func<int> compute = () => Compute();
var (result, took) = compute.Measure();                   // Func<T> ← نتیجه + زمان اجرا
```

### IP Extensions

```csharp
await IPAddress.Parse("8.8.8.8").Ping();
```

### Common: Money

Value object برای مبلغ + واحد پول؛ از جمع/تفریق/مقایسه‌ی دو واحد پول متفاوت به‌صورت type-safe جلوگیری می‌کند.

```csharp
var a = new Money(100_000, "IRR");
var b = new Money(50_000, "IRR");
var total = a + b;                     // Money(150000, "IRR")
a + new Money(10, "USD");              // InvalidOperationException
```

### Common: Result\<T\>

جایگزین throw کردن exception برای مسیرهای خطای قابل‌پیش‌بینی.

```csharp
Result<User> result = userId > 0
    ? Result<User>.Success(user)
    : Result<User>.Failure("کاربر یافت نشد");

if (result.IsSuccess) { /* result.Value */ }
```

### Common: Option\<T\>

مکمل `Result<T>` برای جایی که فقط «هست/نیست» مهمه، نه یک پیام خطای مشخص.

```csharp
Option<User> option = repository.TryFind(id) is { } user
    ? Option<User>.Some(user)
    : Option<User>.None();

option.Match(
    some: user => user.Name,
    none: () => "پیدا نشد");

if (option.TryGetValue(out var value)) { /* value */ }
```

### Common: PagedResult\<T\>

```csharp
PagedResult<Customer> page = customers.ToPagedResult(pageNumber: 2, pageSize: 20);
// page.Items, page.TotalCount, page.TotalPages, page.HasNextPage, page.HasPreviousPage
```

### Common: Validator (Fluent)

بر خلاف Guard که در اولین خطا throw می‌کند، همه‌ی قوانین را چک کرده و لیست کامل خطاها را برمی‌گرداند — مناسب فرم‌هایی که باید همه‌ی خطاها را یک‌جا نشان دهند.

```csharp
var result = Validator<UserDto>.For(dto)
    .Rule(x => x.Name.IsNotEmpty(), "نام الزامی است")
    .Rule(x => x.Mobile.IsValidMobileNumber(), "موبایل نامعتبر است")
    .Validate();

if (!result.IsValid) { /* result.Errors */ }
```

### Common: SimpleCache

کش in-memory ساده با پشتیبانی از expiration؛ برای پروژه‌های کوچکی که نیازی به Redis/MemoryCache ندارند.

```csharp
var cache = new SimpleCache<string, User>();
cache.Set("user:1", user, TimeSpan.FromMinutes(5));
cache.TryGet("user:1", out var cached);
cache.GetOrAdd("user:1", key => LoadUser(key), TimeSpan.FromMinutes(5));
```

### Guard Clauses

```csharp
public void Process(string name, int age)
{
    Guard.Against.Empty(name, nameof(name));
    Guard.Against.Negative(age, nameof(age));
    Guard.Against.OutOfRange(age, 0, 150, nameof(age));
    // ...
}
```

### Data Annotation Attributes

```csharp
public class RegisterDto
{
    [PersianRequired("نام")]
    public string Name { get; set; }

    [PersianMobileNumber]
    public string Mobile { get; set; }

    [PersianNationalCode]
    public string NationalCode { get; set; }

    [IranianIban]
    public string Sheba { get; set; }

    [GreaterThan(0, "سن")]
    public int Age { get; set; }

    [PersianRange(0, 100)]
    public int Score { get; set; }

    [DateRange(nameof(EndDate))]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [RequiredIf(nameof(HasDiscount), true)]
    public decimal? DiscountAmount { get; set; }
    public bool HasDiscount { get; set; }

    [AllowedExtensions(".jpg", ".png")]
    [MaxFileSize(2 * 1024 * 1024)]
    public string AvatarFileName { get; set; }

    [CompareTo(nameof(ConfirmPassword), CompareType.Equal)]
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
```

همه‌ی این attributeها طبق pattern استاندارد `ValidationAttribute` کار می‌کنن (برگرداندن `ValidationResult`/`bool`، نه throw)، پس با `Validator.TryValidateObject` و کتابخانه‌های مبتنی بر DataAnnotations (ASP.NET Core model binding، EF Core و ...) سازگارن.

متادیتای غیر-اعتبارسنجی هم موجوده: `[Note]` (مستندسازی متد)، `[DisplayOrder]`، `[AuditIgnore]`.

### Exceptions

- `AlertException` — پیام قابل‌نمایش مستقیم به کاربر نهایی
- `BadRequestException` — معمولاً باید به HTTP 400 نگاشت بشه (برای خطاهای اعتبارسنجی ورودی در APIها)

---

## ساختار مخزن

```
src/BaseKit/            کد اصلی کتابخانه
tests/BaseKit.Tests/    تست‌های واحد (xUnit، Theory-based)
nupkgs/                 خروجی pack شده (git-ignored)
local-feed/             فید لوکال NuGet برای تست مصرف پکیج (git-ignored)
```

## Build & Pack

```bash
dotnet build
dotnet pack -c Release
```

خروجی `.nupkg` در پوشه‌ی `nupkgs/` قرار می‌گیرد.
