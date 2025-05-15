using System.Globalization;

namespace DDDZamin.Utilities.DateTimes;

/// <summary>
/// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی
/// </summary>
public static class FriendlyPersianDateUtils
{
    /// <summary>
    /// نمایش فارسی روز دریافتی شمسی
    /// </summary>
    /// <param name="persianYear"></param>
    /// <param name="persianMonth"></param>
    /// <param name="persianDay"></param>
    /// <returns></returns>
    public static string ToPersianDateTextify(int persianYear, int persianMonth, int persianDay)
    {
        if (persianYear <= 99)
        {
            persianYear += 1300;
        }

        var strDay = PersianCulture.GetPersianWeekDayName(persianYear, persianMonth, persianDay);
        var strMon = PersianCulture.PersianMonthName[persianMonth];

        return $"{strDay} {persianDay} {strMon} {persianYear}".ToPersianNumbers();
    }

    /// <summary>
    /// نمایش فارسی روز دریافتی، مانند سه‌شنبه ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToPersianDateTextify(this DateTime dt)
    {
        var dateParts = dt.ToPersianYearMonthDay();
        return ToPersianDateTextify(dateParts.Year, dateParts.Month, dateParts.Day);
    }

    /// <summary>
    /// نمایش فارسی روز دریافتی، مانند سه‌شنبه ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToPersianDateTextify(this DateTime? dt) =>
        dt == null
            ? string.Empty
            : dt.Value.ToPersianDateTextify();

    /// <summary>
    /// نمایش فارسی روز دریافتی، مانند سه‌شنبه ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns></returns>
    public static string ToPersianDateTextify(this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart) =>
        dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToPersianDateTextify();

    /// <summary>
    /// نمایش فارسی روز دریافتی، مانند سه‌شنبه ۲۱ دی ۱۳۹۵
    /// </summary>
    /// <param name="dt">تاریخ و زمان</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns></returns>
    public static string ToPersianDateTextify(this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToPersianDateTextify();
    }

    /// <summary>
    /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <param name="comparisonBase">مبنای مقایسه مانند هم‌اکنون</param>
    /// <param name="appendHhMm">آیا ساعت نیز به نتیجه اضافه شود؟</param>
    /// <returns></returns>
    public static string ToFriendlyPersianDateTextify(this DateTime dt, DateTime comparisonBase, bool appendHhMm = true)
    {
        return $"{UniCodeConstants.RleChar}{dt.ToFriendlyPersianDateTextify(comparisonBase, appendHhMm).ToPersianNumbers()}";
    }

    /// <summary>
    /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی،‌مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <param name="appendHhMm"></param>
    /// <returns></returns>
    public static string ToFriendlyPersianDateTextify(this DateTime dt, bool appendHhMm = true)
    {
        var comparisonBase = dt.Kind.GetNow().ToIranTimeZoneDateTime();
        return $"{UniCodeConstants.RleChar}{dt.ToIranTimeZoneDateTime().ToFriendlyPersianDateTextify(comparisonBase, appendHhMm).ToPersianNumbers()}";
    }

    /// <summary>
    /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <param name="comparisonBase">مبنای مقایسه مانند هم‌اکنون</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <param name="appendHhMm">آیا ساعت نیز به نتیجه اضافه شود؟</param>
    /// <returns>نمایش دوستانه</returns>
    public static string ToFriendlyPersianDateTextify(this DateTime dt, DateTime comparisonBase,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime, bool appendHhMm = true)
    {
        return
            $"{UniCodeConstants.RleChar}{dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToFriendlyPersianDateTextify(comparisonBase, appendHhMm).ToPersianNumbers()}";
    }

    /// <summary>
    /// نمایش دوستانه یک تاریخ و ساعت انگلیسی به شمسی، مبنای محاسبه هم‌اکنون، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <param name="appendHhMm">آیا ساعت نیز به نتیجه اضافه شود؟</param>
    /// <returns>نمایش دوستانه</returns>
    public static string ToFriendlyPersianDateTextify(this DateTimeOffset dt, bool appendHhMm = true)
    {
        var comparisonBase = DateTime.UtcNow.ToIranTimeZoneDateTime();
        var iranLocalTime = dt.GetDateTimeOffsetPart(DateTimeOffsetPart.IranLocalDateTime);
        return $"{UniCodeConstants.RleChar}{iranLocalTime.ToFriendlyPersianDate(comparisonBase, appendHhMm).ToPersianNumbers()}";
    }

    /// <summary>
    /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <param name="comparisonBase">مبنای مقایسه مانند هم‌اکنون</param>
    /// <returns>نمایش دوستانه</returns>
    public static string ToFriendlyPersianDateTextify(this DateTime? dt, DateTime comparisonBase)
    {
        return dt == null
            ? string.Empty
            : dt.Value.ToFriendlyPersianDateTextify(comparisonBase);
    }

    /// <summary>
    /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی، مبنای محاسبه هم‌اکنون، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <returns>نمایش دوستانه</returns>
    public static string ToFriendlyPersianDateTextify(this DateTime? dt)
    {
        if (dt == null)
        {
            return string.Empty;
        }

        var comparisonBase = dt.Value.Kind.GetNow().ToIranTimeZoneDateTime();
        return dt.Value.ToIranTimeZoneDateTime().ToFriendlyPersianDateTextify(comparisonBase);
    }

    /// <summary>
    /// نمایش دوستانه یک تاریخ و ساعت انگلیسی به شمسی، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="comparisonBase"></param>
    /// <param name="dateTimeOffsetPart"></param>
    /// <returns></returns>
    public static string ToFriendlyPersianDateTextify(this DateTimeOffset? dt, DateTime comparisonBase,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt == null
            ? string.Empty
            : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToFriendlyPersianDateTextify(comparisonBase);
    }

    /// <summary>
    /// نمایش دوستانه‌ی یک تاریخ و ساعت انگلیسی به شمسی، مبنای محاسبه هم‌اکنون، مانند ۱۰ روز قبل، سه‌شنبه ۲۱ دی ۱۳۹۵، ساعت ۱۰:۲۰
    /// </summary>
    /// <param name="dt">تاریخ ورودی</param>
    /// <returns>نمایش دوستانه</returns>
    public static string ToFriendlyPersianDateTime(this DateTimeOffset? dt)
    {
        if (dt == null)
        {
            return string.Empty;
        }

        var comparisonBase = DateTime.UtcNow.ToIranTimeZoneDateTime();
        var iranLocalTime = dt.Value.GetDateTimeOffsetPart(DateTimeOffsetPart.IranLocalDateTime);
        return iranLocalTime.ToFriendlyPersianDateTime(comparisonBase);
    }

    private static string ToFriendlyPersianDate(this DateTime dt, DateTime comparisonBase, bool appendHhMm)
    {
        var persianDate = dt.ToPersianYearMonthDay();

        var persianYear = persianDate.Year;
        var persianMonth = persianDate.Month;
        var persianDay = persianDate.Day;

        var hour = dt.Hour;
        var minute = dt.Minute;
        var hhMm = $"{hour.ToString("00", CultureInfo.InvariantCulture)}:{minute.ToString("00", CultureInfo.InvariantCulture)}";

        var date = new PersianCalendar().ToDateTime(persianYear, persianMonth, persianDay, hour, minute, 0, 0);
        var diff = date - comparisonBase;
        var totalSeconds = Math.Round(diff.TotalSeconds);
        var totalDays = Math.Round(diff.TotalDays);

        var suffix = " بعد";
        if (totalSeconds < 0)
        {
            suffix = " قبل";
            totalSeconds = Math.Abs(totalSeconds);
            totalDays = Math.Abs(totalDays);
        }

        var dateTimeToday = DateTime.Today;
        var yesterday = dateTimeToday.AddDays(-1);
        var today = dateTimeToday.Date;
        var tomorrow = dateTimeToday.AddDays(1);

        hhMm = appendHhMm
            ? $" ، ساعت {hhMm}"
            : string.Empty;

        if (today == date.Date)
        {
            switch (totalSeconds)
            {
                case < 60:
                    return "هم‌اکنون";
                case < 120:
                    return $"یک دقیقه‌{suffix}{hhMm}";
                case < 3600:
                    return
                        $"{string.Format(CultureInfo.InvariantCulture, "{0} دقیقه", (int)Math.Floor(totalSeconds / 60))}{suffix}{hhMm}";
                case < 7200:
                    return $"یک ساعت‌{suffix}{hhMm}";
                case < 86400:
                    return
                        $"{string.Format(CultureInfo.InvariantCulture, "{0} ساعت", (int)Math.Floor(totalSeconds / 3600))}{suffix}{hhMm}";
            }
        }

        if (yesterday == date.Date)
            return $"دیروز {PersianCulture.GetPersianWeekDayName(persianYear, persianMonth, persianDay)}{hhMm}";

        if (tomorrow == date.Date)
            return $"فردا {PersianCulture.GetPersianWeekDayName(persianYear, persianMonth, persianDay)}{hhMm}";

        var dayString = $"، {ToPersianDateTextify(persianYear, persianMonth, persianDay)}{hhMm}";

        if (totalSeconds < 30 * TimeConstants.Day)
            return $"{(int)Math.Abs(totalDays)} روز{suffix}{dayString}";

        if (totalSeconds < 12 * TimeConstants.Month)
        {
            var months = Convert.ToInt32(Math.Floor((double)Math.Abs(diff.Days) / 30));
            return months <= 1
                ? $"۱ ماه{suffix}{dayString}"
                : $"{months} ماه{suffix}{dayString}";
        }

        var years = Convert.ToInt32(Math.Floor((double)Math.Abs(diff.Days) / 365));
        var daysMonths = (double)Math.Abs(diff.Days) / 30;
        var nextMonths = Convert.ToInt32(Math.Truncate(daysMonths) - years * 12 - 1);
        var nextMonthsString = nextMonths <= 0
            ? ""
            : $"{(years > 1
                ? " و "
                : "")}{nextMonths} ماه";

        return years < 1
            ? $"{nextMonthsString}‌{suffix}‌{dayString}"
            : $"{years} سال‌{nextMonthsString}{suffix}{dayString}";
    }

}