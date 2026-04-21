using Microsoft.Extensions.DependencyInjection;

namespace DDDZamin.Endpoints.Web.Extensions.ModelBinding;

public static class NonValidatingValidatorExtensions
{
    public static IServiceCollection AddNonValidatingValidator(this IServiceCollection services)
    {
        var validator = services.FirstOrDefault(s => s.ServiceType == typeof(IObjectModelValidator));

        if (validator == null) 
            return services;

        services.Remove(validator);

        services.Add(new ServiceDescriptor(typeof(IObjectModelValidator),_=>new NonValidatingValidator(),ServiceLifetime.Singleton));

        return services;
    }
}