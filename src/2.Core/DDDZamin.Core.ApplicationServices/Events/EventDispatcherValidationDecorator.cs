using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zamin.Extensions.Logger.Abstractions;

namespace DDDZamin.Core.ApplicationServices.Events;

public class EventDispatcherValidationDecorator : EventDispatcherDecorator
{
    #region Fileds

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventDispatcherValidationDecorator> _logger;

    public override int Order => 1;

    #endregion

    #region Constructors

    public EventDispatcherValidationDecorator(IServiceProvider serviceProvider,
        ILogger<EventDispatcherValidationDecorator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    #endregion

    #region Publish Domain Event

    public override async Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent @event)
    {
        _logger.LogDebug(ZaminEventId.EventValidation,
            "Validating Event of type {EventType} With value {Event} start at: {StartDateTime}", @event.GetType(),
            @event, DateTime.Now);

        var errorMessages = Validate(@event);

        if (errorMessages.Any())
        {
            _logger.LogInformation(ZaminEventId.EventValidation,
                "Validating query of type {QueryType} With value {Query} failed. Validation errors are: {ValidationErrors}",
                @event.GetType(), @event, errorMessages);
        }
        else
        {
            _logger.LogDebug(ZaminEventId.EventValidation,
                "Validating query of type {QueryType} With value {Query} finished at: {EndDateTime}", @event.GetType(),
                @event, DateTime.Now);

            await _eventDispatcher.PublishDomainEventAsync(@event);
        }
    }

    #endregion

    #region Private Methods

    private List<string> Validate<TDomainEvent>(TDomainEvent @event)
    {
        var errorMessages = new List<string>();

        var validator = _serviceProvider.GetService<IValidator<TDomainEvent>>();

        if (validator != null)
        {
            var validationResult = validator.Validate(@event);

            if (!validationResult.IsValid)
                errorMessages = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
        }
        else
        {
            _logger.LogInformation(ZaminEventId.CommandValidation, "There is not any validator for {EventType}",
                @event.GetType());
        }

        return errorMessages;
    }

    #endregion
}