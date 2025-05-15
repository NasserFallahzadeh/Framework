using System.Text;

namespace DDDZamin.Utilities.Extensions;

public static class StringExtensions
{
    public const char ArabicYeChar = (char)1610;
    public const char PersianYeChar = (char)1740;

    public const char ArabicKeChar = (char)1603;
    public const char PersianKeChar = (char)1705;

    public static string ApplyCorrectYeKe(this object data) =>
        data == null
            ? null
            : ApplyCorrectYeKe(data.ToString());

    public static string ApplyCorrectYeKe(this string data) =>
        string.IsNullOrWhiteSpace(data)
            ? string.Empty
            : data.Replace(ArabicYeChar, PersianYeChar)
                .Replace(ArabicKeChar, PersianKeChar)
                .Trim();

    public static long ToSafeLong(this string input, long replacement = long.MinValue) =>
        long.TryParse(input, out var result)
            ? result
            : replacement;

    public static long? ToSafeNullableLong(this string input) =>
        long.TryParse(input, out var result)
            ? result
            : null;

    public static int ToSafeInt(this string input, int replacement = int.MinValue) =>
        int.TryParse(input, out var result)
            ? result
            : replacement;

    public static int? ToSafeNullableInt(this string input) =>
        int.TryParse(input, out var result)
            ? result
            : null;

    public static string ToStringOrEmpty(this string? input) =>
        input ?? string.Empty;

    public static string ToUnderScoreCase(this string input) =>
        string.Concat(input.Select((x, i) => i > 0 &&
                                             char.IsUpper(x)
                ? $"_{x.ToString()}"
                : x.ToString()))
            .ToLower();

    public static byte[] ToByteArray(this string input) => Encoding.UTF8.GetBytes(input);

    public static string FromByteArray(this byte[] input) => Encoding.UTF8.GetString(input);
}