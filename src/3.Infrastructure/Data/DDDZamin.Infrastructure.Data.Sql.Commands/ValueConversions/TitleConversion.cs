using System.Linq.Expressions;
using DDDZamin.Core.Domain.Toolkits.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDZamin.Infrastructure.Data.Sql.Commands.ValueConversions;

public class TitleConversion:ValueConverter<Title,string>
{
    public TitleConversion(Expression<Func<Title, string>> convertToProviderExpression, Expression<Func<string, Title>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public TitleConversion(Expression<Func<Title, string>> convertToProviderExpression, Expression<Func<string, Title>> convertFromProviderExpression, bool convertsNulls, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}