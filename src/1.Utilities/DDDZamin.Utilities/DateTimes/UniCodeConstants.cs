namespace DDDZamin.Utilities.DateTimes;

public static class UniCodeConstants
{
    public const char RleChar = (char)0x202B;

    /// <summary>
    /// Applies RLE to the text if it contains Persian words.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ApplyRle(this string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        return text.ContainsFarsi()
            ? $"{RleChar}{text}"
            : text;
    }
}