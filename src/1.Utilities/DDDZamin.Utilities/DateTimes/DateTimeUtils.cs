using System.Runtime.InteropServices;

namespace DDDZamin.Utilities.DateTimes;

/// <summary>
/// متدهای کمکی جهت کار با تاریخ میلادی
/// </summary>
public static class DateTimeUtils
{
    /// <summary>
    /// Iran Standard Time
    /// </summary>
    public static readonly TimeZoneInfo IranStandardTime;

    /// <summary>
    /// Epoch represented as DateTime
    /// </summary>
    public static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    static DateTimeUtils()
    {
        IranStandardTime = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(timeZoneInfo =>
            timeZoneInfo.StandardName.Contains("Iran") ||
            timeZoneInfo.StandardName.Contains("Tehran") ||
            timeZoneInfo.Id.Contains("Iran") ||
            timeZoneInfo.Id.Contains("Tehran"));
        if (IranStandardTime == null)
        {
#if NET40 || NET45 || NET46
            throw new PlatformNotSupportedException(
                $"This OS [{Environment.OSVersion.Platform},{Environment.OSVersion.Version}] doesn't support IranStandardTime.");
#else
            throw new PlatformNotSupportedException(
                $"This OS [{RuntimeInformation.OSDescription}] doesn't support IranStandardTime.");
#endif
        }
    }

    /// <summary>
    /// محاسبه سن
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <param name="comparisonBase">مبنای محاسبه مانند هم‌اکنون</param>
    /// <param name="dateTimeOffsetPart">کدام جزء این وهله مورد استفاده قرار گیرد؟</param>
    /// <returns></returns>
    public static int GetAge(this DateTimeOffset birthday, DateTime comparisonBase,
        DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime) =>
        birthday.GetDateTimeOffsetPart(dateTimeOffsetPart).GetAge(comparisonBase);

    /// <summary>
    /// محاسبه سن، مبنای محاسبه هم‌اکنون
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTimeOffset birthday)
    {
        var birthdayDateTime = birthday.UtcDateTime;
        var now = DateTime.UtcNow;
        return birthdayDateTime.GetAge(now);
    }

    /// <summary>
    /// محاسبه سن
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <param name="comparisonBase">مبنای محاسبه مانند هم‌اکنون</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTime birthday, DateTime comparisonBase)
    {
        var age = comparisonBase.Year - birthday.Year;
        if (comparisonBase < birthday.AddYears(age))
            age--;

        return age;
    }

    /// <summary>
    /// محاسبه سن، مبنای محاسبه هم‌اکنون
    /// </summary>
    /// <param name="birthday">تاریخ تولد</param>
    /// <returns>سن</returns>
    public static int GetAge(this DateTime birthday)
    {
        var now = birthday.Kind.GetNow();
        return birthday.GetAge(now);
    }

    /// <summary>
    /// دریافت جزء زمانی ویژه این وهله
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    /// <param name="dataDateTimeOffsetPart"></param>
    /// <returns></returns>
    public static DateTime GetDateTimeOffsetPart(this DateTimeOffset dateTimeOffset,
        DateTimeOffsetPart dataDateTimeOffsetPart)
    {
        switch (dataDateTimeOffsetPart)
        {
            case DateTimeOffsetPart.DateTime:
                return dateTimeOffset.DateTime;

            case DateTimeOffsetPart.LocalDateTime:
                return dateTimeOffset.LocalDateTime;

            case DateTimeOffsetPart.UtcDateTime:
                return dateTimeOffset.UtcDateTime;

            case DateTimeOffsetPart.IranLocalDateTime: return dateTimeOffset.ToIranTimeZoneDateTimeOffset().DateTime;

            default:
                throw new ArgumentOutOfRangeException(nameof(dataDateTimeOffsetPart), dataDateTimeOffsetPart, null);
        }
    }

    /// <summary>
    /// بازگشت زمان جاری با توجه به نوع زمان
    /// </summary>
    /// <param name="dataDateTimeKind">نوع زمان ورودی</param>
    /// <returns></returns>
    public static DateTime GetNow(this DateTimeKind dataDateTimeKind)
    {
        return dataDateTimeKind switch
        {
            DateTimeKind.Utc => DateTime.UtcNow,
            _ => DateTime.Now
        };
    }

    /// <summary>
    /// تبدیل منطقه زمانی این وهله به منطقه زمانی ایران
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    /// <returns></returns>
    public static DateTimeOffset ToIranTimeZoneDateTimeOffset(this DateTimeOffset dateTimeOffset) =>
        TimeZoneInfo.ConvertTime(dateTimeOffset, IranStandardTime);

    /// <summary>
    /// تبدیل منطقه زمانی این وهله به منطقه زمانی ایران
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime ToIranTimeZoneDateTime(this DateTime dateTime) =>
        dateTime;

    /// <summary>
    /// Converts a given <see cref="DateTime"/> to milliseconds from Epoch
    /// </summary>
    /// <param name="dateTime">A given <see cref="DateTime"/></param>
    /// <returns>Milliseconds since Epoch</returns>
    public static long ToEpochMilliseconds(this DateTime dateTime) =>
        (long)dateTime.ToUniversalTime().Subtract(Epoch).TotalMilliseconds;

    /// <summary>
    /// Converts a given <see cref="DateTime"/> to seconds from Epoch.
    /// </summary>
    /// <param name="dateTime">A given <see cref="DateTime"/></param>
    /// <returns>The Unix time stamp</returns>
    public static long ToEpochSeconds(this DateTime dateTime) => 
        dateTime.ToEpochSeconds() / 1000;

    /// <summary>
    /// Checks the given date is between the two provided dates
    /// </summary>
    /// <param name="date"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="compareTime"></param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate, bool compareTime = false)
    {
        return compareTime
            ? date >= startDate &&
              date <= endDate
            : date.Date >= startDate.Date &&
              date.Date <= endDate.Date;
    }

    /// <summary>
    /// Returns whether the given date is the last day of the month
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static bool IsLastDayOfTheMonth(this DateTime dateTime) => 
        dateTime == new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);

    /// <summary>
    /// Returns whether the given date falls in a weekend
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsWeekend(this DateTime value) => 
        value.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday;

    /// <summary>
    /// Determines if a given year is a LeapYear or not.
    /// </summary>
    /// <param name="value"></param>
    public static bool IsLeapYear(this DateTime value) => 
        DateTime.DaysInMonth(value.Year, 2) == 29;

    //Converts to a DateTimeOffset
    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeSpan offset) =>
        dt==DateTime.MinValue 
            ? DateTimeOffset.MinValue 
            : new DateTimeOffset(dt.Ticks, offset);

    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, double offsetInHours = 0)
    {
        return dt.ToDateTimeOffset(offsetInHours == 0
            ? TimeSpan.Zero
            : TimeSpan.FromHours(offsetInHours));
    }
}