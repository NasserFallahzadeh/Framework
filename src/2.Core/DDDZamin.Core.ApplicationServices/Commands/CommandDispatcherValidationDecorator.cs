using DDDZamin.Core.RequestResponse.Commands;
using DDDZamin.Core.RequestResponse.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zamin.Extensions.Logger.Abstractions;

namespace DDDZamin.Core.ApplicationServices.Commands;

public class CommandDispatcherValidationDecorator : CommandDispatcherDecorator
{
    #region Fileds

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandDispatcherValidationDecorator> _logger;

    public override int Order => 1;

    #endregion

    #region Constructors

    public CommandDispatcherValidationDecorator(IServiceProvider serviceProvider, ILogger<CommandDispatcherValidationDecorator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    #endregion

    #region Send Commands

    public override async Task<CommandResult> Send<TCommand>(TCommand command)
    {
        _logger.LogDebug(ZaminEventId.CommandValidation,
            "Validation command of type {CommandType} With value {Command} start at :{StartDateTime}",
            command.GetType(), command, DateTime.Now);

        var validationResult = Validate<TCommand, CommandResult>(command);

        if (validationResult != null)
        {
            _logger.LogInformation(ZaminEventId.CommandValidation, "Validation command of type {CommandType} With value {Command} falied. Validation errors are: {ValidationErrors}", command.GetType(), command, validationResult.Messages);

            return validationResult;
        }

        _logger.LogDebug(ZaminEventId.CommandValidation,
            "Validation command of type {CommandType} With value {Command} finished at :{EndDateTime}",
            command.GetType(), command, DateTime.Now);

        return await _commandDispatcher.Send(command);
    }

    public override async Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command)
    {
        _logger.LogDebug(ZaminEventId.CommandValidation,
            "Validation command of type {CommandType} With value {Command} start at :{StartDateTime}",
            command.GetType(), command, DateTime.Now);

        var validationResult = Validate<TCommand, CommandResult<TData>>(command);

        if (validationResult != null)
        {
            _logger.LogInformation(ZaminEventId.CommandValidation,
                "Validation command of type {Commandtype} With value {Command} failed. Validation errors are: {ValidationErrors}",
                command.GetType(), command, validationResult.Messages);

            return validationResult;
        }

        _logger.LogDebug(ZaminEventId.CommandValidation, "Validation command of type {CommandType} With value {Command} finished at: {EndDateTime}", command.GetType(), command, DateTime.Now);

        return await _commandDispatcher.Send<TCommand, TData>(command);
    }

    #endregion

    #region Private Methods

    private TValidationResult Validate<TCommand, TValidationResult>(TCommand command)
        where TValidationResult : ApplicationServiceResult, new()
    {
        var validator = _serviceProvider.GetService<IValidator<TCommand>>();

        TValidationResult res = null;

        if (validator != null)
        {
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid)
            {
                res = new TValidationResult
                {
                    Status = ApplicationServiceStatus.ValidationError
                };

                foreach (var error in validationResult.Errors) res.AddMessage(error.ErrorMessage);
            }
        }
        else
            _logger.LogInformation(ZaminEventId.CommandValidation, "There is not any validator for {CommandType}",
                command.GetType());

        return res;
    }

    #endregion
}