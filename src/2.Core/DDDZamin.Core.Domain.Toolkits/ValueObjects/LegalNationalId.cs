using DDDZamin.Core.Domain.Exceptions;
using DDDZamin.Core.Domain.ValueObjects;
using DDDZamin.Utilities.Extensions;

namespace DDDZamin.Core.Domain.Toolkits.ValueObjects;

public class LegalNationalId:BaseValueObject<LegalNationalId>
{
    #region Properties

    public string Value { get; private set; }

    #endregion

    #region Constructors and Factories

    public static LegalNationalId FromString(string value) => new(value);

    private LegalNationalId(string value)
    {
        if (!value.IsLegalNationalIdValid())
            throw new InvalidValueObjectStateException("ValidationErrorStringFormat", nameof(LegalNationalId));

        Value = value;
    }

    public LegalNationalId()
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

    public static explicit operator string(LegalNationalId title) => title.Value;

    public static implicit operator LegalNationalId(string value) => new(value);

    #endregion

    #region Meghods

    public override string ToString() => 
        Value;

    #endregion
}