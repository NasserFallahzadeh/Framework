namespace Miniblog.Core.Domain.Resources;

public class MessagePattern
{
    public static string EmptyStringValidationMessage = "The value for {0} could not be null";
    public static string StringLengthValidationMessage = "The length of {0} should be between {1} and {2}";
    public static string IdValidationMessage = "The value for id could not be less than 1";
    public static string FirstName = nameof(FirstName);
    public static string LastName = nameof(LastName);
}