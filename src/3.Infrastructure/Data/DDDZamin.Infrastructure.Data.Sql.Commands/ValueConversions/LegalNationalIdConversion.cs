using System.Linq.Expressions;
using DDDZamin.Core.Domain.Toolkits.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDZamin.Infrastructure.Data.Sql.Commands.ValueConversions;

public class LegalNationalIdConversion:ValueConverter<LegalNationalId,string>
{
    public LegalNationalIdConversion(Expression<Func<LegalNationalId, string>> convertToProviderExpression, Expression<Func<string, LegalNationalId>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public LegalNationalIdConversion(Expression<Func<LegalNationalId, string>> convertToProviderExpression, Expression<Func<string, LegalNationalId>> convertFromProviderExpression, bool convertsNulls, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}