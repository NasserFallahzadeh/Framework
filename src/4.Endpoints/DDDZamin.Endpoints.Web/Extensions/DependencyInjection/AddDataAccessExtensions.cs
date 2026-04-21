using System.Reflection;
using DDDZamin.Core.Contracts.Data.Commands;
using DDDZamin.Core.Contracts.Data.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace DDDZamin.Endpoints.Web.Extensions.DependencyInjection;

public static class AddDataAccessExtensions
{
    public static IServiceCollection AddZaminDataAccess(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddRepositories(assembliesForSearch).AddUnitOfWorks(assembliesForSearch);

    public static IServiceCollection AddRepositories(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) => services.AddTransientLifetime(assembliesForSearch,
        typeof(ICommandRepository<,>), typeof(IQueryRepository));

    public static IServiceCollection AddUnitOfWorks(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(IUnitOfWork));
}