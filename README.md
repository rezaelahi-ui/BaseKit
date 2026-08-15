# BaseKit

🇮🇷 [مطالعه به فارسی](README.fa.md)

A collection of extension methods, attributes, and helper utilities meant to be shared across multiple projects (from .NET Framework 4.6.1 to .NET 10). The main focus is on the needs of Persian/Iranian projects (Shamsi/Jalali dates, national code/mobile/IBAN(Sheba) validation, Persian digits) alongside general-purpose tools (Guard clauses, Result pattern, pagination, simple caching, and more).

## Install

```bash
dotnet add package BaseKit
```

Target frameworks: `netstandard2.0` (compatible with .NET Framework 4.6.1+ and .NET Core/5+) and `net6.0` (for features like nullable reference type detection, available only in .NET 6+; projects on .NET 7 through 10 automatically use the net6.0-specific build too).

---

## Table of contents

- [String Extensions](#string-extensions)
- [Numeric Extensions](#numeric-extensions)
- [Comparable Extensions](#comparable-extensions)
- [Date Extensions (Shamsi dates)](#date-extensions-shamsi-dates)
- [Validation Extensions](#validation-extensions)
- [Fuzzy Matching](#fuzzy-matching)
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
"".IsEmpty();                          // true — null/empty/whitespace-only
"value".IsNotEmpty();                  // true

"1,234".ToInt();                       // 1234 (supports comma thousands separator)
"1,234.5".ToDecimal();                 // 1234.5m
"1,234.5".ToDouble();                  // 1234.5
"123456789012".ToLong();               // 123456789012
"192.168.1.1".ToIp();                  // IPAddress
"https://example.com".ToUri();         // Uri (must start with http/https)
"1".ToBool();                          // true (1/0, true/false, yes/no, بله/خیر)

"123".ToPersianDigits();               // "۱۲۳"
"۱۲۳".ToEnglishDigits();                // "123" (supports both Persian and Arabic digits)
"كتاب".NormalizeArabicChars();          // "کتاب" (Arabic ي/ك → Persian ی/ک)

"09123456789".Mask();                  // "0912***6789"
"یک متن طولانی است".Truncate(10);       // keeps whole words + "..."

"hgt".ToPersianKeyboard();             // "الف" (fixes text typed with the wrong keyboard layout)
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
5.Between(1, 10);                      // true — inclusive on both ends
DateTime.Today.Between(rangeStart, rangeEnd);
"m".Between("a", "z");                 // works for any IComparable<T> (numbers, dates, strings, Money, ...)
```

### Date Extensions (Shamsi dates)

```csharp
DateTime.Now.ToShamsi();               // "1402/01/01"
DateTime.Now.ToClock();                // "13:05:09"
"1402/01/01".ToGregorian();            // DateTime
"1402/01/01".IsValidShamsiDate();      // true
"1402/02/01".IsGreaterThan("1402/01/01"); // string-based date comparison

// working days (Thursday + Friday as weekend by default, configurable)
DateTime.Today.IsWeekend();
DateTime.Today.NextWorkingDay();
DateTime.Today.AddWorkingDays(5);

DateTime.Now.GetPersianMonthName();    // "فروردین"
DateTime.Now.GetPersianDayName();      // "شنبه"
DateTime.Now.GetPersianSeason();       // "بهار"
DateTime.Now.GetShamsiYear();          // 1402 (as int, no string parsing needed)
DateTime.Now.GetShamsiMonth();         // 1

// official Iranian holidays: weekend + fixed-date holidays (Nowruz, ...).
// Lunar/religious holidays shift every year, so pass them in explicitly per year.
DateTime.Today.IsIranianHoliday();
DateTime.Today.IsIranianHoliday(extraHolidaysShamsi: new[] { "1402/06/06" }); // e.g. Ashura for that year

// rich info about the month/season/year a date falls in (PersianDateInfo)
PersianDateInfo info = DateTime.Now.GetPersianDateInfo();
// info.Year, info.MonthName, info.SeasonName, info.DaysInMonth,
// info.MonthStartShamsi/MonthEndShamsi, info.SeasonStartDate/SeasonEndDate, ...

List<WeekInfo> weeks = 1402.GetWeeksOfShamsiMonth(month: 1);     // weeks of Farvardin 1402 (starting Saturday)
List<MonthInfo> months = 1402.GetMonthsOfShamsiSeason(seasonStartMonth: 1); // the 3 months of spring 1402
List<SeasonInfo> seasons = 1402.GetSeasonsOfShamsiYear();        // the 4 seasons of 1402

"1402/01/01".ToUnixTimestamp("13:05:09"); // combine a Shamsi date + time string into a Unix ms timestamp (UTC)
```

### Validation Extensions

```csharp
"0499370899".IsValidNationalCode();     // Iranian national code validation with check-digit algorithm
"09123456789".IsValidMobileNumber();    // Iranian mobile number
"test@example.com".IsValidEmail();
"DE89370400440532013000".IsValidIban(); // IBAN/Sheba with standard mod-97 algorithm

"1234567890".IsValidPostalCode();       // Iranian 10-digit postal code (format only, no official check-digit exists)
"12345678918".IsValidLegalNationalId(); // legal-entity (company) national ID — different check-digit algorithm than personal IDs

"6037-9900-0000-0006".IsValidCardNumber(); // 16-digit bank card, Luhn algorithm
"6037990000000006".GetBankName();          // "بانک ملی ایران" (from the BIN; known/common banks only, returns null if unrecognized)
"IR120170000000000000000000".GetBankNameFromIban(); // bank name from the 3-digit bank code inside the IBAN

"12ب34567".IsValidPlateNumber();        // Iranian license plate format
```

### Fuzzy Matching

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
MyEnum.Value.Humanize();               // from [Description] or the enum name
MyEnum.Value.ToInt();
MyEnum.Value.GetAllNames();
MyEnum.Value.GetDetails(withAll: true); // List<EnumDetail> for dropdowns
"Value".ToEnum<MyEnum>();              // safe parsing with a Persian error message
2.ToEnum<MyEnum>();
```

### Collection Extensions

```csharp
list.IsEmpty();
list.ForEach(x => Console.WriteLine(x));
items.ChunkBy(3);                      // split into chunks of 3 (deliberately not named like .NET 6+'s Chunk, to avoid clashing)
items.DistinctByKey(x => x.Id);        // (deliberately not named like .NET 6+'s DistinctBy)
items.Page(pageNumber: 2, pageSize: 20);
items.ToPagedResult(pageNumber: 2, pageSize: 20); // PagedResult<T> with TotalPages/HasNextPage/...
oldList.HasChanges(newList);

list.Shuffle();                        // in-place Fisher-Yates shuffle
items.RandomItem();                    // a random item from the sequence
```

### Object / Reflection Extensions

```csharp
var clone = myObject.Clone();          // deep clone via JSON serialize/deserialize
var dict = myObject.ToDictionary();    // Dictionary<string, object?> of public properties
```

### Exception Extensions

```csharp
exception.GetFullMessage();            // full message including all InnerExceptions
```

### Task Extensions

```csharp
await someTask.WithTimeout(TimeSpan.FromSeconds(5));   // throws TimeoutException on timeout

Func<Task<int>> operation = () => CallExternalServiceAsync();
await operation.RetryAsync(retryCount: 3, delay: TimeSpan.FromSeconds(1));
await operation.RetryWithBackoffAsync(retryCount: 5); // exponential backoff + jitter between attempts

// waits for every task even if some fail, and collects ALL their exceptions
// (unlike Task.WhenAll, which only surfaces the first one)
await tasks.WhenAllSafe();
```

### File Extensions

```csharp
@"C:\logs\app".EnsureDirectoryExists();
"report:2024/06.pdf".GetSafeFileName(); // strips characters not allowed in file names
```

### Debug / Logging Extensions

```csharp
myObject.Dump();                       // readable JSON for quick debugging
myObject.ToJson();
json.FromJson<MyDto>();

Action action = () => DoSomeWork();
TimeSpan elapsed = action.Measure();                      // Action → just the elapsed time

Func<int> compute = () => Compute();
var (result, took) = compute.Measure();                   // Func<T> → result + elapsed time
```

### IP Extensions

```csharp
await IPAddress.Parse("8.8.8.8").Ping();
```

### Common: Money

A value object for an amount + currency; prevents type-unsafe addition/subtraction/comparison between two different currencies.

```csharp
var a = new Money(100_000, "IRR");
var b = new Money(50_000, "IRR");
var total = a + b;                     // Money(150000, "IRR")
a + new Money(10, "USD");              // InvalidOperationException
```

### Common: Result\<T\>

An alternative to throwing exceptions for predictable error paths.

```csharp
Result<User> result = userId > 0
    ? Result<User>.Success(user)
    : Result<User>.Failure("کاربر یافت نشد");

if (result.IsSuccess) { /* result.Value */ }
```

### Common: Option\<T\>

Companion to `Result<T>` for when only "is there a value or not" matters, not a specific error message.

```csharp
Option<User> option = repository.TryFind(id) is { } user
    ? Option<User>.Some(user)
    : Option<User>.None();

option.Match(
    some: user => user.Name,
    none: () => "not found");

if (option.TryGetValue(out var value)) { /* value */ }
```

### Common: PagedResult\<T\>

```csharp
PagedResult<Customer> page = customers.ToPagedResult(pageNumber: 2, pageSize: 20);
// page.Items, page.TotalCount, page.TotalPages, page.HasNextPage, page.HasPreviousPage
```

### Common: Validator (Fluent)

Unlike Guard, which throws on the first error, this checks every rule and returns the full list of errors — useful for forms that need to display all errors at once.

```csharp
var result = Validator<UserDto>.For(dto)
    .Rule(x => x.Name.IsNotEmpty(), "نام الزامی است")
    .Rule(x => x.Mobile.IsValidMobileNumber(), "موبایل نامعتبر است")
    .Validate();

if (!result.IsValid) { /* result.Errors */ }
```

### Common: SimpleCache

A simple in-memory cache with expiration support; for small projects that don't need Redis/MemoryCache.

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

All of these attributes follow the standard `ValidationAttribute` pattern (returning `ValidationResult`/`bool` instead of throwing), so they work with `Validator.TryValidateObject` and any DataAnnotations-based library (ASP.NET Core model binding, EF Core, etc.).

Non-validation metadata is also available: `[Note]` (method documentation), `[DisplayOrder]`, `[AuditIgnore]`.

### Exceptions

- `AlertException` — a message meant to be shown directly to the end user
- `BadRequestException` — typically maps to HTTP 400 (for input validation errors in APIs)

---

## Repository layout

```
src/BaseKit/            core library code
tests/BaseKit.Tests/    unit tests (xUnit, Theory-based)
nupkgs/                 packed output (git-ignored)
local-feed/             local NuGet feed for testing package consumption (git-ignored)
```

## Build & Pack

```bash
dotnet build
dotnet pack -c Release
```

The `.nupkg` output is placed in the `nupkgs/` folder.
