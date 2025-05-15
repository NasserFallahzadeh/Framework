using System.Text.RegularExpressions;

namespace DDDZamin.Utilities.DateTimes;

/// <summary>
/// متد‌های کمکی مبتنی بر عبارات با‌قاعده
/// </summary>
public static class RegexUtils
{
    /// <summary>
    /// زمان انقضای پردازش عبارت با‌قاعده
    /// </summary>
    public static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(3);

    private static readonly Regex _matchAllTags =
        new Regex(@"<(.|\n)*?>", options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
            , matchTimeout: MatchTimeout
#endif
        );

    private static readonly Regex _matchArabicHebrew = new Regex(@"[\u0600-\u06FF,\u0590-\u05FF,«,»]",
        options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
        , matchTimeout: MatchTimeout
#endif
    );

    private static readonly Regex _matchOnlyPersianNumbersRange = new Regex(@"^[\u06F0-\u06F9]+$",
        options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
        , matchTimeout: MatchTimeout
#endif
    );

    private static readonly Regex _matchOnlyPersianLetters = new Regex(
        @"^[\\s,\u06A9\u06AF\u06C0\u06CC\u7060C,\u062A\u062B\u062C\u062D\u062E\u062F,\u063A\u064A\u064B\u064C\u064D\u064E,\u064F\u067E\u0670\u0686\u200C,\u0621-\u0629\u0630-\u0639\u0641-u-654]+$",
        options: RegexOptions.Compiled | RegexOptions.IgnoreCase
#if !NET40
        , matchTimeout: MatchTimeout
#endif
    );

    /// <summary>
    /// آیا عبارت مدنظر حاوی حروف و اعداد فارسی است؟
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static bool ContainsFarsi(this string txt) =>
        !string.IsNullOrEmpty(txt) &&
        _matchArabicHebrew.IsMatch(txt.StripHtmlTags().Replace(",", ""));

    /// <summary>
    /// آیا عبارت مد‌نظر فقط حاوی حروف فارسی است؟
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static bool ContainsOnlyFarsiLetters(this string txt) =>
        !string.IsNullOrEmpty(txt) &&
        _matchOnlyPersianLetters.IsMatch(txt.StripHtmlTags().Replace(",", ""));

    /// <summary>
    /// حذف تگ‌های یک عمارت
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string StripHtmlTags(this string text) =>
        string.IsNullOrEmpty(text)
            ? string.Empty
            : _matchAllTags.Replace(text, " ").Replace("&nbsp;", " ");

    /// <summary>
    /// اگر متن شما حاوی عبارت فارسی باشد آن را داخل یک بلاک اج تی ام ال را به چپ محصور می‌کند
    /// <div style='text-align: right; font-family: {fontFamily}; font-size:{fontSize};' dir='rtl'>{body}</div>
    /// در غیر این صورت
    /// <div style='text-align: left; font-family:{fontFamily}; font-size:{fontSize};' dir='ltr'>{body}</div>
    /// </summary>
    /// <param name="body"></param>
    /// <param name="fontFamily"></param>
    /// <param name="fontSize"></param>
    /// <returns></returns>
    public static string WrapInDicrectionalDiv(this string body, string fontFamily = "tahoma", string fontSize = "9pt")
    {
        if (string.IsNullOrWhiteSpace(body))
            return string.Empty;

        return body.ContainsFarsi() 
            ? $"<div style='text-align: right; font-family:{fontFamily}; font-size:{fontSize};' dir='rtl'>{body}</div>" 
            : $"<div style='text-align: left; font-family:{fontFamily}; font-size:{fontSize};' dir='ltr'>{body}</div>";
    }

    public static bool ContainsOnlyPersianNumbers(this string text)
    {
        return !string.IsNullOrEmpty(text) &&
               _matchOnlyPersianNumbersRange.IsMatch(text.StripHtmlTags());
    }
}