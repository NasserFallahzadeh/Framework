using System.Collections;
using System.Globalization;
using System.Reflection;

namespace DDDZamin.Utilities.DateTimes;

/// <summary>
/// فرهنگ فارسی سفارشی‌سازی شده
/// </summary>
public static class PersianCulture
{
    /// <summary>
    /// معادل فارسی روز‌های هفته میلادی
    /// </summary>
    public static readonly IDictionary<DayOfWeek, string> PersianDayWeekNames = new
        Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Saturday, "شنبه" },
            { DayOfWeek.Sunday, "یک‌شنبه" },
            { DayOfWeek.Monday, "دوشنبه" },
            { DayOfWeek.Tuesday, "سه‌شنبه" },
            { DayOfWeek.Wednesday, "جهارشنبه" },
            { DayOfWeek.Thursday, "پنج‌شنبه" },
            { DayOfWeek.Friday, "جمعه" }
        };

    /// <summary>
    /// عدد به حروف روز‌های شمسی
    /// </summary>
    public static readonly IDictionary<byte, string> PersianMonthDayNumberNames = new Dictionary<byte, string>
    {
        { 1, "یکم" },
        { 2, "دوم" },
        { 3, "سوم" },
        { 4, "چهارم" },
        { 5, "پنجم" },
        { 6, "ششم" },
        { 7, "هفتم" },
        { 8, "هشتم" },
        { 9, "نهم" },
        { 10, "دهم" },
        { 11, "یازدهم" },
        { 12, "دوازدهم" },
        { 13, "سیزدهم" },
        { 14, "چهاردهم" },
        { 15, "پانزدهم" },
        { 16, "شانزدهم" },
        { 17, "هفدهم" },
        { 18, "هجدهم" },
        { 19, "نوزدهم" },
        { 20, "بیستم" },
        { 21, "بیست‌و‌یکم" },
        { 22, "بیست‌و‌دوم" },
        { 23, "بیست‌و‌سوم" },
        { 24, "بیست‌و‌چهارم" },
        { 25, "بیست‌و‌پنجم" },
        { 26, "بیست‌و‌ششم" },
        { 27, "بیست‌وهفتم" },
        { 28, "بیست‌و‌هشتم" },
        { 29, "بیست‌و‌نهم" },
        { 30, "سی‌ام" },
        { 31, "سی‌و‌یکم" }
    };

    /// <summary>
    /// نام فارسی ماه‌های شمسی
    /// </summary>
    public static readonly Dictionary<byte, string> PersianMonthNames = new Dictionary<byte, string>
    {
        { 1, "فروردین" },
        { 2, "اردیبهشت" },
        { 3, "خرداد" },
        { 4, "تیر" },
        { 5, "مرداد" },
        { 6, "شهریور" },
        { 7, "مهر" },
        { 8, "آبان" },
        { 9, "آذر" },
        { 10, "دی" },
        { 11, "بهمن" },
        { 12, "اسفند" }
    };

    public static readonly Lazy<CultureInfo> _cultureInfoBuilder = new(GetPersianCulture, LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    /// وهله‌ی یکتای فرهنگ فارسی سفارشی‌سازی شده
    /// </summary>
    public static CultureInfo Instance { get; } = _cultureInfoBuilder.Value;

    /// <summary>
    /// Returns the day-of-month part of this <see cref="DateTime"/> localized in persian calendar
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to extend.</param>
    /// <returns>An integer between 1 and 31 representing the day-of-month part of this <see cref="DateTime"/>.</returns>
    public static int GetPersianDayOfMonth(this DateTime dateTime) => Instance.DateTimeFormat.Calendar.GetDayOfMonth(dateTime);

    /// <summary>
    /// Returns the month part of this <see cref="DateTime"/> localized in persian calendar
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to extend.</param>
    /// <returns>An integer between 1 and 12 representing the month part this <see cref="DateTime"/>.</returns>
    public static int GetPersianMonth(this DateTime dateTime) => Instance.DateTimeFormat.Calendar.GetMonth(dateTime);

    /// <summary>
    /// عدد به حروف روز‌های شمسی
    /// </summary>
    /// <param name="dayNumber"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetPersianMonthDayNumberName(this byte dayNumber)
    {
        if (dayNumber is < 1 or > 31)
            throw new ArgumentOutOfRangeException($"{nameof(dayNumber)} must be between 1, 31.");

        return PersianMonthDayNumberNames[dayNumber];
    }

    /// <summary>
    /// نام فارسی ماه‌های شمسی
    /// </summary>
    /// <param name="monthNumber"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetPersianMonthName(this byte monthNumber)
    {
        if (monthNumber is < 1 or > 12)
            throw new ArgumentOutOfRangeException($"{nameof(monthNumber)} must be between 1, 12");

        return PersianMonthNames[monthNumber];
    }

    /// <summary>
    /// دریافت معادل فارسی نام روز هفته میلادی
    /// </summary>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static string GetPersianWeekDayName(this DayOfWeek dayOfWeek) => 
        PersianDayWeekNames[dayOfWeek];

    /// <summary>
    /// گرفتن نام فارسی روز‌های هفته
    /// </summary>
    /// <param name="persianYear"></param>
    /// <param name="persianMonth"></param>
    /// <param name="persianDay"></param>
    /// <returns></returns>
    public static string GetPersianWeekDayName(int persianYear, int persianMonth, int persianDay) =>
        PersianDayWeekNames[new PersianCalendar().ToDateTime(persianYear, persianMonth, persianDay, 0, 0, 0, 0).DayOfWeek];

    /// <summary>
    /// گرفتن نام فارسی روز‌های هفته
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string GetPersianWeekDayName(this DateTime dt)
    {
        var dateParts = dt.ToPersianYearMonthDay();
        return PersianDayWeekNames[
            new PersianCalendar().ToDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute,dt.Second, dt.Millisecond).DayOfWeek];
    }

    /// <summary>
    /// گرفتن نام فارسی روز‌های هفته
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string GetPersianWeekDayName(this DateTime? dt) =>
        dt == null
            ? string.Empty
            : dt.Value.GetPersianWeekDayName();

    /// <summary>
    /// گرفتن نام فارسی روز‌های هفته
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="dateTimeOffsetPart"></param>
    /// <returns></returns>
    public static string GetPersianWeekDayName(this DateTimeOffset? dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).GetPersianWeekDayName();
    }

    /// <summary>
    /// گرفتن نام فارسی روز‌های هفته
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="dateTimeOffsetPart"></param>
    /// <returns></returns>
    public static string GetPersianWeekDayName(this DateTimeOffset dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt.GetDateTimeOffsetPart(dateTimeOffsetPart).GetPersianWeekDayName();

    /// <summary>
    /// Returns the year part of this <see cref="DateTime"/> localized in persian calendar.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to extend.</param>
    /// <returns>An integer between 1 and 9999 representing the year part of this <see cref="DateTime"/>.</returns>
    public static int GetPersianYear(this DateTime dateTime) => Instance.DateTimeFormat.Calendar.GetYear(dateTime);

    /// <summary>
    /// تاریخ روز‌های ابتدا و انتهای سال شمسی را بازگشت می‌دهد
    /// </summary>
    /// <param name="persianYear"></param>
    /// <returns></returns>
    public static PersianYear GetPersianYearStartAndEndDates(this int persianYear)
    {
        var persianCalendar = new PersianCalendar();
        return new PersianYear
        {
            StartDate = persianCalendar.ToDateTime(persianYear, 1, 1, 0, 0, 0, 0),
            EndDate = persianCalendar.ToDateTime(persianYear, 12, persianYear.GetPersianMonthLastDay(12), 23, 59, 59, 0)
        };
    }

    /// <summary>
    /// سال شمسی معادل را محاسبه کرده و سپس تاریخ روز‌های ابتدا و انتهای آن سال را بازگشت می‌دهد
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    /// <param name="dateTimeOffsetPart"></param>
    /// <returns></returns>
    public static PersianYear GetPersianYearStartAndEndDates(this DateTimeOffset dateTimeOffset,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        var persianYear = dateTimeOffset.GetDateTimeOffsetPart(dateTimeOffsetPart).GetPersianYear();
        return persianYear.GetPersianYearStartAndEndDates();
    }

    /// <summary>
    /// تاریخ روز‌های ابتدا و انتهای ما شمسی را بازگشت می‌دهد
    /// </summary>
    /// <param name="persianYear"></param>
    /// <param name="persianMonth"></param>
    /// <returns></returns>
    public static PersianMonth GetPersianMonthStartAndEndDates(this int persianYear, int persianMonth)
    {
        var persianCalendar=new PersianCalendar();
        var isLeapYear = persianCalendar.IsLeapYear(persianYear);
        return new PersianMonth
        {
            StartDate = persianCalendar.ToDateTime(persianYear, persianMonth, 1, 0, 0, 0, 0),
            EndDate = persianCalendar.ToDateTime(persianYear, persianMonth,
                persianYear.GetPersianMonthLastDay(persianMonth), 23, 59, 59, 0)
        };
    }

    /// <summary>
    /// ماه شمسی معادل را محاسبه کرده و سپس تاریخ روز‌های ابتدا و انتهای آن ما شمسی را بازگشت می‌دهد
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static PersianMonth GetPersianMonthStartAndEndDates(this DateTime dateTime)
    {
        var persianYear = dateTime.GetPersianYear();
        var persianMonth = dateTime.GetPersianMonth();
        return persianYear.GetPersianMonthStartAndEndDates(persianMonth);
    }

    /// <summary>
    /// ماه شمسی معادل را محاسبه کرده و سپس تاریخ روز‌های ابتدا و انتهای آن ماه شمسی را بازگشت می‌دهد
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    /// <param name="dateTimeOffsetPart"></param>
    /// <returns></returns>
    public static PersianMonth GetPersianMonthStartAndEndDates(this DateTimeOffset dateTimeOffset,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        var dateTime = dateTimeOffset.GetDateTimeOffsetPart(dateTimeOffsetPart);
        var persianYear = dateTime.GetPersianYear();
        var persianMonth = dateTime.GetPersianMonth();
        return persianYear.GetPersianMonthStartAndEndDates(persianMonth);
    }

    /// <summary>
    /// شماره آخرین روز ماه شمسی را بر‌می‌گرداند
    /// </summary>
    /// <param name="persianYear">سال شمسی</param>
    /// <param name="persianMonth">ماه شمسی</param>
    /// <returns>شماره آخرین روز ماه</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static int GetPersianMonthLastDay(this int persianYear, int persianMonth)
    {
        switch (persianMonth)
        {
            case > 12 or <= 0:
                throw new ArgumentOutOfRangeException("ماه وارد شده معتبر نیست.");
            case <= 6:
                return 31;
            case 12:
            {
                var persianCalendar = new PersianCalendar();
                return persianCalendar.IsLeapYear(persianYear)
                    ? 30
                    : 29;
            }
            default:
                return 30;
        }
    }

    /// <summary>
    /// اصلاح تقویم فرهنگ فارسی
    /// </summary>
    /// <returns></returns>
    private static CultureInfo GetPersianCulture()
    {
        var persianCulture = new CultureInfo("fa-IR")
        {
            DateTimeFormat =
            {
                AbbreviatedDayNames = ["ی", "د", "س", "چ", "پ", "ج", "ش"],
                AbbreviatedMonthGenitiveNames =
                [
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند"
                ],
                AMDesignator = "ق.ظ",
                CalendarWeekRule = CalendarWeekRule.FirstDay,
                DateSeparator = "/",
                DayNames = ["یک‌شنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"],
                FirstDayOfWeek = DayOfWeek.Saturday,
                FullDateTimePattern = "dddd dd MMMM yyyy",
                LongDatePattern = "dd MMMM yyyy",
                LongTimePattern = "h:mm:ss tt",
                MonthDayPattern = "dd MMMM",
                MonthGenitiveNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند"
                },
                MonthNames =
                [
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند"
                ],
                PMDesignator = "ب.ظ",
                ShortDatePattern = "yyyy/MM/dd",
                ShortestDayNames = ["ی", "د", "س", "چ", "پ", "ج", "ش"],
                ShortTimePattern = "HH:mm",
                TimeSeparator = ":",
                YearMonthPattern = "MMMM yyyy"
            }
        };

        var persianCalendar = new PersianCalendar();
        var fieldInfo = persianCulture
            .GetType()
            .GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);

        fieldInfo?.SetValue(persianCulture, persianCalendar);

        var info = persianCulture.DateTimeFormat
            .GetType()
            .GetField("calendar",
                BindingFlags.NonPublic | BindingFlags.Instance);

        info?.SetValue(persianCulture.DateTimeFormat, persianCalendar);

        persianCulture.NumberFormat.NumberDecimalSeparator = "/";
        persianCulture.NumberFormat.NumberNegativePattern = 0;

        return persianCulture;
    }
}