using System.Globalization;
using DDDZamin.Core.Domain.Toolkits.ValueObjects;
using DDDZamin.Core.Domain.ValueObjects;
using DDDZamin.Infrastructure.Data.Sql.Commands.Extensions;
using DDDZamin.Infrastructure.Data.Sql.Commands.ValueConversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DDDZamin.Infrastructure.Data.Sql.Commands;

public class BaseCommandDbContext:DbContext
{
    protected IDbContextTransaction _transaction;

    public BaseCommandDbContext(DbContextOptions options):base(options)
    {
        
    }

    protected BaseCommandDbContext()
    {
        
    }

    public void BeginTransaction()
    {
        _transaction = Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        if (_transaction == null)
            throw new NullReferenceException("Please call BeginTransaction() method first.");

        _transaction.Rollback();
    }

    public void CommitTransaction()
    {
        if (_transaction == null)
            throw new NullReferenceException("Please call BeginTransaction() method first.");

        _transaction.Commit();
    }

    public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
    {
        var value = Entry(entity).Property(propertyName).CurrentValue;
        return value != null
            ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
            : default;
    }

    public object GetShadowPropertyValue(object entity, string propertyName)
    {
        return Entry(entity).Property(propertyName).CurrentValue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddAudittableShadowProperties();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Description>().HaveConversion<DescriptionConversion>();

        configurationBuilder.Properties<Title>().HaveConversion<TitleConversion>();

        configurationBuilder.Properties<BusinessId>().HaveConversion<BusinessIdConversion>();

        configurationBuilder.Properties<LegalNationalId>().HaveConversion<LegalNationalIdConversion>();

        configurationBuilder.Properties<NationalCode>().HaveConversion<NationalCodeConversion>();
    }

    public IEnumerable<string> GetIncludePaths(Type clrEntityType)
    {
        var entityType = Model.FindEntityType(clrEntityType);

        var includeNavigations = new HashSet<INavigation>();

        var stack = new Stack<IEnumerator<INavigation>>();

        while (true)
        {
            var entityNavigations = new List<INavigation>();
            foreach (var navigation in entityType.GetNavigations())
            {
                if (includeNavigations.Add(navigation))
                    entityNavigations.Add(navigation);
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(s => s.Current.Name));
                }
                else
                {
                    foreach (var entityNavigation in entityNavigations)
                    {
                        var InverseNavigation = entityNavigation.Inverse;
                        if (InverseNavigation != null)
                            includeNavigations.Add(InverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }

                while (stack.Count > 0 &&
                       !stack.Peek().MoveNext())
                    stack.Pop();

                if(stack.Count==0)
                    break;

                entityType = stack.Peek().Current.TargetEntityType;
            }
        }
    }
}