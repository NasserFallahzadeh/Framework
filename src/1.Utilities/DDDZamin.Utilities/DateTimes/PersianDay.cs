namespace DDDZamin.Utilities.DateTimes;

/// <summary>
/// اجزای روز شمسی
/// </summary>
public class PersianDay
{
    /// <summary>
    /// سال شمسی
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// ماه شمسی
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// روز شمسی
    /// </summary>
    public int Day { get; set; }

    /// <summary>
    /// اجزای روز شمسی
    /// </summary>
    public PersianDay()
    {

    }

    /// <summary>
    /// اجزای روز شمسی
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    public PersianDay(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public override string ToString() =>
        $"{Year}/{Month:00}/{Day:00}";

    public override bool Equals(object? obj)
    {
        if (obj is not PersianDay day)
            return false;

        return Year == day.Year &&
               Month == day.Month &&
               Day == day.Day;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Year.GetHashCode();
            hash = hash * 23 + Month.GetHashCode();
            hash = hash * 23 + Day.GetHashCode();
            return hash;
        }
    }
}