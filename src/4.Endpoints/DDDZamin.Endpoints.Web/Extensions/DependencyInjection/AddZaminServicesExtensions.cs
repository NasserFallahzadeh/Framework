using DDDZamin.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace DDDZamin.Endpoints.Web.Extensions.DependencyInjection;

public static class AddZaminServicesExtensions
{
    public static IServiceCollection AddZaminUtilityServices(this IServiceCollection services)
    {
        services.AddTransient<ZaminServices>();
        return services;
    }
}