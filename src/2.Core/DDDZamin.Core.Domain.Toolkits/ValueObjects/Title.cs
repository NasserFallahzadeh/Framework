using DDDZamin.Core.Domain.Exceptions;
using DDDZamin.Core.Domain.ValueObjects;

namespace DDDZamin.Core.Domain.Toolkits.ValueObjects;

public class Title:BaseValueObject<Title>
{
    #region Properties

    public string Value { get; private set; }

    #endregion

    #region Constructors and Factories

    public static Title FromString(string value) => new(value);

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectStateException("ValidationErrorIsRequire {0}", nameof(Title));

        if (value.Length is < 2 or > 250)
            throw new InvalidValueObjectStateException("ValidationErrorStringLength {0} {1} {2}", nameof(Title), "2",
                "250");

        Value = value;
    }

    public Title()
    {
        
    }

    #endregion

    #region Equality Check

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    #endregion

    #region Operator Overloading

    public static explicit operator string(Title title) => title.Value;

    public static implicit operator Title(string  title) => new(title);

    #endregion

    #region Methods

    public override string ToString() => Value;

    #endregion
}