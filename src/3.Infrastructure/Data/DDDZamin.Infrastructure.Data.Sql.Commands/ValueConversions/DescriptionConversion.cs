using System.Linq.Expressions;
using DDDZamin.Core.Domain.Toolkits.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDZamin.Infrastructure.Data.Sql.Commands.ValueConversions;

public class DescriptionConversion:ValueConverter<Description,string>
{
    public DescriptionConversion(Expression<Func<Description, string>> convertToProviderExpression, Expression<Func<string, Description>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public DescriptionConversion(Expression<Func<Description, string>> convertToProviderExpression, Expression<Func<string, Description>> convertFromProviderExpression, bool convertsNulls, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}