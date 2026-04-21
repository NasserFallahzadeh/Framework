using System.Globalization;

namespace DDDZamin.Utilities.DateTimes;

public static class PersianDateTimeUtils
{
    /// <summary>
    /// تعیین اعتبار تاریخ شمسی
    /// </summary>
    /// <param name="persianYear">سال شمسی</param>
    /// <param name="persianMonth">ماه شمسی</param>
    /// <param name="persianDay">روز شمسی</param>
    /// <returns></returns>
    public static bool IsValidPersianDate(int persianYear, int persianMonth, int persianDay)
    {
        if (persianDay is > 31 or <= 0)
            return false;

        switch (persianMonth)
        {
            case > 12 or <= 0:
            case <= 6 when
                persianDay > 31:
            case >= 7 when
                persianDay > 30:
                return false;
            case 12:
                {
                    var persianCalendar = new PersianCalendar();
                    var isLeapYear = persianCalendar.IsLeapYear(persianYear);

                    switch (isLeapYear)
                    {
                        case true when
                            persianYear > 30:
                        case false when
                            persianDay > 29:
                            return false;
                    }

                    break;
                }
        }

        return true;
    }

    /// <summary>
    /// تعیین اعتبار تاریخ و زمان رشته‌ای شمسی، با قالب‌های پشتیبانی شده‌ی ۹۰/۸/۱۴ , ۱۳۹۵/۱۱/۳ 17:50 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴
    /// </summary>
    /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
    /// <returns></returns>
    public static bool IsValidPersianDateTime(this string persianDateTime)
    {
        try
        {
            var dt = persianDateTime.ToGregorianDateTime();
            return dt.HasValue;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// تبدیل تاریخ و زمان رشته‌ای شمسی به میلادی، با قالب‌های پشتیبانی شده‌ی 90/8/14 , ۱۳۹۵/۱۱/۳ 17:30 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴در اینجا اگر رشته‌ی مدنظر قابل تبدیل نباشد، مقدار نال را دریافت خواهید کرد
    /// </summary>
    /// <param name="persianDateTime">تاریخ و زمان میلادی</param>
    /// <returns></returns>
    public static DateTime? ToGregorianDateTime(this string persianDateTime)
    {
        if (string.IsNullOrWhiteSpace(persianDateTime))
            return null;

        persianDateTime = persianDateTime.Trim().ToEnglishNumbers();
        var splitedDateTime = persianDateTime.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var rawTime = splitedDateTime.FirstOrDefault(s => s.Contains(":"));
        var rawDate = splitedDateTime.FirstOrDefault(s => !s.Contains(':'));

        var splitedDate = rawDate?.Split('/', ',', '/', '.', '-');
        if (splitedDate?.Length != 3)
            return null;

        var day = GetDay(splitedDate[2]);
        if (!day.HasValue)
            return null;

        var month = GetMonth(splitedDate[1]);
        if (!month.HasValue)
            return null;

        var year = GetYear(splitedDate[0]);
        if (!year.HasValue)
            return null;

        if (!IsValidPersianDate(year.Value, month.Value, day.Value))
            return null;

        var hour = 0;
        var minute = 0;
        var second = 0;

        if (!string.IsNullOrWhiteSpace(rawTime))
        {
            var splitedTime = rawTime.Split(new[] { ':' },
                StringSplitOptions.RemoveEmptyEntries);
            hour = int.Parse(splitedTime[0]);
            minute = int.Parse(splitedTime[1]);
            if (splitedTime.Length > 2)
            {
                var lastPart = splitedTime[2].Trim();
                var formatInfo = PersianCulture.Instance.DateTimeFormat;
                if (lastPart.Equals(formatInfo.PMDesignator, StringComparison.OrdinalIgnoreCase))
                {
                    if (hour < 12)
                        hour += 12;
                }
                else
                    int.TryParse(lastPart, out second);
            }
        }

        var persianCalendar = new PersianCalendar();
        return persianCalendar.ToDateTime(year.Value, month.Value, day.Value, hour, minute, second, 0);
    }

    /// <summary>
    /// تبدیل تاریخ و زمان رشته‌ای شمسی به میلادی با قالب‌های پشتیبانی شده‌ی
    /// </summary>
    /// <param name="persianDateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset? ToGregorianDateTimeOffset(this string persianDateTime)
    {
        var dateTime = persianDateTime.ToGregorianDateTime();

        if (dateTime == null)
            return null;

        return new DateTimeOffset(dateTime.Value, DateTimeUtils.IranStandardTime.BaseUtcOffset);
    }

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToLongPersianDateString(this DateTime dt) => dt.ToPersianDateTimeString(PersianCulture.Instance.DateTimeFormat.LongDatePattern);

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToLongPersianDateString(this DateTime? dt) =>
        dt == null
            ? string.Empty
            : dt.Value.ToLongPersianDateString();

    public static string ToLongPersianDateString(this DateTimeOffset? dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToLongPersianDateString(this DateTimeOffset dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۲۱ دی ۱۳۹۵، ۱۰:۲۰:۰۲ ق.ظ
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToLongPersianDateTimeString(this DateTime dt) => dt.ToPersianDateTimeString(
        $"{PersianCulture.Instance.DateTimeFormat.LongDatePattern}، {PersianCulture.Instance.DateTimeFormat.LongTimePattern}");

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۲۱ دی ۱۳۹۵، ۱۰:۲۰:۰۲ ق.ظ
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <returns>تاریه شمسی</returns>
    public static string ToLongPersianDateTimeString(this DateTime? dt) =>
        dt == null
            ? string.Empty
            : dt.Value.ToLongPersianDateTimeString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۲۱ دی ۱۳۹۵، ۱۰:۲۰:۰۲ ق.ظ
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToLongPersianDateTimeString(this DateTimeOffset? dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateTimeString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToLongPersianDateTimeString(this DateTimeOffset dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateTimeString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="format"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToPersianDateTimeString(this DateTime dateTime, string format) => dateTime.ToString(format, PersianCulture.Instance);

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه حاصل
    /// </summary>
    /// <param name="gregorianDate">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns></returns>
    public static PersianDay ToPersianYearMonthDay(this DateTimeOffset? gregorianDate,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        gregorianDate == null
            ? throw new ArgumentNullException(nameof(gregorianDate))
            : gregorianDate.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToPersianYearMonthDay();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ماه و روز نتیجه حاصل
    /// </summary>
    /// <param name="gregorianDate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static PersianDay ToPersianYearMonthDay(this DateTime? gregorianDate) =>
        gregorianDate == null
            ? throw new ArgumentNullException(nameof(gregorianDate))
            : gregorianDate.Value.ToPersianYearMonthDay();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی ودریافت اجزای سال، ماه و روز نتیجه‌ی حاصل
    /// </summary>
    /// <param name="gregorianDate">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار بگیرد؟</param>
    /// <returns></returns>
    public static PersianDay ToPersianDayYearMonthDay(this DateTimeOffset gregorianDate,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        gregorianDate.GetDateTimeOffsetPart(dateTimeOffsetPart).ToPersianYearMonthDay();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی و دریافت اجزای سال، ‌ماه و روز نتیجه‌ی حاصل
    /// </summary>
    /// <param name="gregorianDate"></param>
    /// <returns></returns>
    public static PersianDay ToPersianYearMonthDay(this DateTime gregorianDate)
    {
        var persianCalendar = new PersianCalendar();
        var persianYear = persianCalendar.GetYear(gregorianDate);
        var persianMonth = persianCalendar.GetMonth(gregorianDate);
        var persianDay = persianCalendar.GetDayOfMonth(gregorianDate);
        return new PersianDay
        {
            Year = persianYear,
            Month = persianMonth,
            Day = persianDay
        };
    }

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns></returns>
    public static string ToShortPersianDateString(this DateTimeOffset? dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار می‌گیرد؟</param>
    /// <returns></returns>
    public static string ToShortPersianDateString(this DateTimeOffset dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToShortPersianDateString(this DateTime dt) =>
        dt.ToPersianDateTimeString(PersianCulture.Instance.DateTimeFormat.ShortDatePattern);

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToShortPersianDateString(this DateTime? dt) =>
        dt == null
            ? string.Empty
            : dt.Value.ToShortPersianDateString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToShortPersianDateTimeString(this DateTime dt) =>
        dt.ToPersianDateTimeString(
            $"{PersianCulture.Instance.DateTimeFormat.ShortDatePattern}{PersianCulture.Instance.DateTimeFormat.ShortTimePattern}");

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۰:۲۰ ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToShortPersianDateTimeString(this DateTime? dt) =>
        dt == null
            ? string.Empty
            : dt.Value.ToShortPersianDateTimeString();

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۰:۲۰ ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns>تاریخ شمسی</returns>
    public static string ToShortPersianDateTimeString(this DateTimeOffset? dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateTimeString();
    }

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با قالبی مانند ۱۰:۲۰ ۱۳۹۵/۱۰/۲۱
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="dateTimeOffsetPart"></param>
    /// <returns></returns>
    public static string ToShortPersianDateTimeString(this DateTimeOffset dt,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateTimeString();

    private static int? GetDay(string part)
    {
        var day = part.ToNumber();
        if (!day.Item1)
            return null;

        var pDay = day.Item2;
        if (pDay is 0 or > 31)
            return null;

        return pDay;
    }

    private static int? GetMonth(string part)
    {
        var month = part.ToNumber();
        if (!month.Item1)
            return null;

        var pMonth = month.Item2;
        if (pMonth is 0 or > 31)
            return null;

        return pMonth;
    }

    private static int? GetYear(string part)
    {
        var year = part.ToNumber();
        if (!year.Item1)
            return null;

        var pYear = year.Item2;
        if (part.Length == 2)
            pYear += 1300;

        return pYear;
    }

    private static Tuple<bool, int> ToNumber(this string data)
    {
        var result = int.TryParse(data, out var number);

        return new Tuple<bool, int>(result, number);
    }

    /// <summary>
    /// رشته شامل تاریخ شمسی به صورت سال/ماه/روز را به روز/ماه/سال تبدیل می‌کند
    /// </summary>
    /// <param name="reverseDate">رشته تاریخ شمسی معکوس</param>
    /// <returns></returns>
    public static string CorrectReverseDate(string reverseDate)
    {
        reverseDate = reverseDate.Trim();
        var dateParts = reverseDate.Split('/', ',', '/', '.', '-', '\\');

        return dateParts is [_, _, { Length: 4 }] 
            ? $"{dateParts[2]}/{dateParts[1]}/{dateParts[0]}" 
            : reverseDate;
    }
}