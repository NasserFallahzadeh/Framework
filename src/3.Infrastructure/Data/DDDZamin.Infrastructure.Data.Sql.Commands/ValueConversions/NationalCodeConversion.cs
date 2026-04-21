using System.Linq.Expressions;
using DDDZamin.Core.Domain.Toolkits.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDZamin.Infrastructure.Data.Sql.Commands.ValueConversions;

public class NationalCodeConversion:ValueConverter<NationalCode,string>
{
    public NationalCodeConversion(Expression<Func<NationalCode, string>> convertToProviderExpression, Expression<Func<string, NationalCode>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public NationalCodeConversion(Expression<Func<NationalCode, string>> convertToProviderExpression, Expression<Func<string, NationalCode>> convertFromProviderExpression, bool convertsNulls, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}