using System.Linq.Expressions;
using DDDZamin.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDZamin.Infrastructure.Data.Sql.Commands.ValueConversions;

public class BusinessIdConversion:ValueConverter<BusinessId,Guid>
{
    public BusinessIdConversion(Expression<Func<BusinessId, Guid>> convertToProviderExpression, Expression<Func<Guid, BusinessId>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public BusinessIdConversion(Expression<Func<BusinessId, Guid>> convertToProviderExpression, Expression<Func<Guid, BusinessId>> convertFromProviderExpression, bool convertsNulls, ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}