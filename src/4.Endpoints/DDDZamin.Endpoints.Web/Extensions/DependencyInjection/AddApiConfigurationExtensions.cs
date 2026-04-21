using DDDZamin.Endpoints.Web.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDZamin.Endpoints.Web.Extensions.DependencyInjection;

public static class AddApiConfigurationExtensions
{
    public static IServiceCollection AddZaminApiCore(this IServiceCollection services,
        params string[] assemblyNamesForLoad)
    {
        services.AddControllers(delegate (MvcOptions options)
        {
            options.Filters.Add<PascalCaseJsonFilter>();
            options.Filters.Add<CamelCaseJson>();
        }).AddFluentValidation();

        services.AddZaminDependencies(assemblyNamesForLoad);

        return services;
    }

    public static void UseZaminApiExceptionHandler(this IApplicationBuilder app)
    {
        app.UseApiExceptionHandler(options =>
        {
            options.AddResponseDetails = (context, exception, error) =>
            {
                if (exception.GetType().Name == typeof(SqlException).Name)
                {
                    error.Detail = "Exception was a database exception!";
                }
            };

            options.DetermineLogLevel = exception =>
            {
                if (exception.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
                    exception.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
                {
                    return LogLevel.Critical;
                }

                return LogLevel.Error;
            };
        });
    }
}