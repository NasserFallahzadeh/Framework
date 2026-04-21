using DDDZamin.Core.Domain.Exceptions;
using DDDZamin.Core.Domain.ValueObjects;
using DDDZamin.Utilities.Extensions;
using Miniblog.Core.Domain.Resources;

namespace Miniblog.Core.Domain.People.ValueObjects;

public class FirstName:BaseValueObject<FirstName>
{
    public string Value { get; private set; }

    public static FirstName FromString(string value) => new(value);

    public FirstName(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new InvalidValueObjectStateException(MessagePattern.EmptyStringValidationMessage, nameof(FirstName));

        if (value.IsLengthBetween(2,50))
            throw new InvalidValueObjectStateException(MessagePattern.StringLengthValidationMessage, nameof(FirstName),"2","50");

        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(FirstName title) => title.Value;
    public static implicit operator FirstName(string value) => new(value);
}