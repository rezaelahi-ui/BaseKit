# BaseKit

متدهای Extension مشترک (string, object, ...) برای استفاده در پروژه‌های مختلف — از .NET Framework 4.6.1 تا .NET 10.

## ساختار

```
src/BaseKit/            کد اصلی کتابخانه
tests/BaseKit.Tests/    تست‌های واحد
nupkgs/                 خروجی pack شده (git-ignored)
local-feed/             فید لوکال NuGet برای تست مصرف پکیج (git-ignored)
```

## Build & Pack

```bash
dotnet build
dotnet pack -c Release
```

خروجی `.nupkg` در پوشه `nupkgs/` قرار می‌گیرد.
