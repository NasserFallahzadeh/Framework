namespace DDDZamin.Utilities.DateTimes;

/// <summary>
/// DateTimeOffset Part
/// سورس این بخش از ابتدا از مجموعه کار‌های مهنس وحید نصیری برداشته شده است. در صورت نیاز تغییرات لازم رو انجام می‌دهم
/// </summary>
public enum DateTimeOffsetPart
{
    /// <summary>
    /// قسمت زمان مقدار را بدون توجه به آفست باز می‌گرداند و به زبان محلی سرور تبدیل نخواهد شد
    /// </summary>
    DateTime,

    /// <summary>
    /// قسمت زمان را با توجه به منظقه زمانی سروری که برنامه بر روی آن اجرا می‌شود، برمی‌گرداند
    /// </summary>
    LocalDateTime,

    /// <summary>
    /// The Coordinated Universal Time (UTC) date and time of the current System.DateTimeOffset
    /// </summary>
    UtcDateTime,

    /// <summary>
    /// این وهله را به منطقه زمانی ایران تبدیل و مقدار را بر‌می‌گرداند
    /// </summary>
    IranLocalDateTime
}