using System.Reflection;
using DDDZamin.Core.ApplicationServices.Commands;
using DDDZamin.Core.ApplicationServices.Events;
using DDDZamin.Core.ApplicationServices.Queries;
using DDDZamin.Core.Contracts.ApplicationServices.Commands;
using DDDZamin.Core.Contracts.ApplicationServices.Events;
using DDDZamin.Core.Contracts.ApplicationServices.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace DDDZamin.Endpoints.Web.Extensions.DependencyInjection;

public static class AddApplicationServiceExtensions
{
    public static IServiceCollection
        AddZaminApplicationServices(this IServiceCollection services, IEnumerable<Assembly> assembliesForSearch) =>
        services.AddCommandHandlers(assembliesForSearch).AddCommandDispatcherDecorators()
            .AddQueryHandlers(assembliesForSearch).AddQueryDispatcherDecorators().AddEventHandlers(assembliesForSearch)
            .AddEventDispatcherDecorators().AddFluentValidation(assembliesForSearch);

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) => services.AddWithTransientLifetime(assembliesForSearch,
        typeof(ICommandHandler<>), typeof(ICommandHandler<,>));

    private static IServiceCollection AddCommandDispatcherDecorators(this IServiceCollection services)
    {
        services.AddTransient<CommandDispatcher, CommandDispatcher>();

        services.AddTransient<CommandDispatcherDecorator, CommandDispatcherDomainExceptionHandlerDecorator>();

        services.AddTransient<CommandDispatcherDecorator, CommandDispatcherValidationDecorator>();

        services.AddTransient<ICommandDispatcher>(c =>
        {
            var commandDispatcher = c.GetRequiredService<CommandDispatcher>();

            var decorators = c.GetServices<CommandDispatcherDecorator>().ToList();

            if (!decorators.Any()) return commandDispatcher;
            decorators = decorators.OrderBy(c => c.Order).ToList();

            var listFinalIndex = decorators.Count - 1;

            for (var i = 0; i < decorators.Count; i++)
                if (i == listFinalIndex)
                    decorators[i].SetCommandDispatcher(commandDispatcher);
                else
                    decorators[i].SetCommandDispatcher(decorators[i + 1]);

            return decorators[0];

        });

        return services;
    }

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(IQueryHandler<,>));

    private static IServiceCollection AddQueryDispatcherDecorators(this IServiceCollection services)
    {
        services.AddTransient<QueryDispatcher, QueryDispatcher>();

        services.AddTransient<QueryDispatcherDomainExceptionHandlerDecorator>();

        services.AddTransient<QueryDispatcherDecorator, QueryDispatcherValidationDecorator>();

        services.AddTransient<IQueryDispatcher>(c =>
        {
            var queryDispatcher = c.GetRequiredService<QueryDispatcher>();

            var decorators = c.GetServices<QueryDispatcherDecorator>().ToList();

            if (!decorators.Any())
                return queryDispatcher;

            decorators = decorators.OrderBy(c => c.Order).ToList();

            var listFinalIndex = decorators.Count - 1;

            for (int i = 0; i < decorators.Count; i++)
                if (i == listFinalIndex)
                    decorators[i].SetQueryDispatcher(queryDispatcher);
                else
                    decorators[i].SetQueryDispatcher(decorators[i + 1]);

            return decorators[0];
        });

        return services;
    }

    private static IServiceCollection AddEventHandlers(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddWithTransientLifetime(assembliesForSearch, typeof(IDomainEventHandler<>));

    private static IServiceCollection AddEventDispatcherDecorators(this IServiceCollection services)
    {
        services.AddTransient<EventDispatcher, EventDispatcher>();

        services.AddTransient<EventDispatcherDecorator, EventDispatcherDomainExceptionHandlerDecorator>();

        services.AddTransient<EventDispatcherDecorator, EventDispatcherValidationDecorator>();

        services.AddTransient<IEventDispatcher>(c =>
        {
            var queryDispatcher = c.GetRequiredService<EventDispatcher>();

            var decorators = c.GetServices<EventDispatcherDecorator>().ToList();

            if (!decorators.Any())
                return queryDispatcher;

            decorators = decorators.OrderBy(c => c.Order).ToList();

            var listFinalIndex = decorators.Count - 1;

            for (int i = 0; i < decorators.Count; i++)
            {
                if (i == listFinalIndex)
                    decorators[i].SetEventDispatcher(queryDispatcher);

                else
                    decorators[i].SetEventDispatcher(decorators[i - 1]);
            }

            return queryDispatcher;
        });

        return services;
    }

    private static IServiceCollection AddFluentValidators(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) => services.AddValidatorsFromAssemblies(assembliesForSearch);
}