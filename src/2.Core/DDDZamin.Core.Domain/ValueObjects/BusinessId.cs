using DDDZamin.Core.Domain.Exceptions;

namespace DDDZamin.Core.Domain.ValueObjects;

public class BusinessId:BaseValueObject<BusinessId>
{
    public Guid Value { get; private init; }

    public static BusinessId FromString(string value) => new(value);

    public static BusinessId FromGuid(Guid value) => new() { Value = value };

    private BusinessId(){}

    public BusinessId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectStateException("ValidationErrorIsRequired", nameof(BusinessId));

        if (Guid.TryParse(value, out var tempValue))
            Value = tempValue;
        else
            throw new InvalidValueObjectStateException("ValidationErrorInvalidValue", nameof(BusinessId));
    }

    public override string ToString() => 
        Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(BusinessId title) => title.Value.ToString();
    public static implicit operator BusinessId(string value) => new(value);

    public static explicit operator Guid(BusinessId title) => title.Value;
    public static implicit operator BusinessId(Guid value) => new() { Value = value };
}