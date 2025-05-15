using DDDZamin.Core.Domain.Exceptions;
using DDDZamin.Core.Domain.ValueObjects;
using DDDZamin.Utilities.Extensions;
using Miniblog.Core.Domain.Resources;

namespace Miniblog.Core.Domain.People.ValueObjects;

public class LastName : BaseValueObject<LastName>
{
    public string Value { get; private set; }

    public static LastName FromString(string value) => new(value);

    public LastName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidValueObjectStateException(MessagePattern.EmptyStringValidationMessage, nameof(LastName));
        }

        if (value.IsLengthBetween(2,50))
        {
            throw new InvalidValueObjectStateException(MessagePattern.StringLengthValidationMessage, nameof(LastName), "2", "50");
        }
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(LastName title) => title.Value;
    public static implicit operator LastName(string value) => new(value);
}