using System.ComponentModel;

namespace DDDZamin.Utilities.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// برای دریافت توضیحات یک ویژگی از enum اگر [Description] داشته باشد از این متد استفاده می‌شود.
    /// </summary>
    /// <param name="enumValue">مقداری که قرار است توضیحات آن دریافت شود</param>
    /// <returns>متن داخل [Description] در صورتی که وجود داشته باشد و در غیر این صورت عنوان ارسال شده</returns>
    public static string GetEnumDescription(this Enum enumValue)
    {
        var memberInfo = enumValue.GetType().GetField(enumValue.ToString());

        var attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        var description = attributes != null
            ? ((DescriptionAttribute)attributes.FirstOrDefault()).Description
            : enumValue.ToString();

        return description;
    }
}