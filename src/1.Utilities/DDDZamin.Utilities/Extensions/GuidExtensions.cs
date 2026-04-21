namespace DDDZamin.Utilities.Extensions;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid) =>
        guid == null || 
        guid == Guid.Empty;

    public static bool IsEmpty(this Guid guid) =>
        guid == Guid.Empty;
}