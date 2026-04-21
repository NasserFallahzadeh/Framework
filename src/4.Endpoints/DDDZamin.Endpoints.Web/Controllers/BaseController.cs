using DDDZamin.Core.Contracts.ApplicationServices.Commands;
using DDDZamin.Core.Contracts.ApplicationServices.Events;
using DDDZamin.Core.Contracts.ApplicationServices.Queries;
using DDDZamin.Core.RequestResponse.Commands;
using DDDZamin.Core.RequestResponse.Common;
using DDDZamin.Core.RequestResponse.Queries;
using DDDZamin.Utilities;
using System.Net;
using Zamin.Extensions.Serializers.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DDDZamin.Endpoints.Web.Controllers;

public class BaseController : Controller
{
    protected ICommandDispatcher CommandDispatcher => HttpContext.CommandDispatcher();

    protected IQueryDispatcher QueryDispatcher => HttpContext.QueryDispatcher();

    protected IEventDispatcher EventDispatcher => HttpContext.EventDispatcher();

    protected ZaminServices ZaminApplicationContext => HttpContext.ZaminApplicationContext();

    public IActionResult Excel<T>(List<T> list, string fileName)
    {
        var serializer = (IExcelSerializer)HttpContext.RequestServices.GetRequiredService(typeof(IExcelSerializer));

        var bytes = serializer.ListToExcelByteArray(list);

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
    }

    protected async Task<IActionResult> Create<TCommand, TCommandResult>(TCommand command)
        where TCommand : class, ICommand<TCommandResult>
    {
        var result = await CommandDispatcher.Send<TCommand, TCommandResult>(command);

        if (result.Status == ApplicationServiceStatus.Ok)
        {
            return StatusCode(HttpStatusCode.Created.GetHashCode(), result.Data);
        }

        return BadRequest(result.Messages);
    }

    protected async Task<IActionResult> Create<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await CommandDispatcher.Send(command);

        if (result.Status == ApplicationServiceStatus.Ok)
        {
            return StatusCode(HttpStatusCode.Created.GetHashCode());
        }

        return BadRequest(result.Messages);
    }

    protected async Task<IActionResult> Edit<TCommand, TCommandResult>(TCommand command)
        where TCommand : class, ICommand<TCommandResult>
    {
        var result = await CommandDispatcher.Send<TCommand, TCommandResult>(command);

        return result.Status switch
        {
            ApplicationServiceStatus.Ok => StatusCode(HttpStatusCode.OK.GetHashCode(), result.Data),
            ApplicationServiceStatus.NotFound => StatusCode(HttpStatusCode.NotFound.GetHashCode(), command),
            _ => BadRequest(result.Messages)
        };
    }

    protected async Task<IActionResult> Edit<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await CommandDispatcher.Send(command);

        return result.Status switch
        {
            ApplicationServiceStatus.Ok => StatusCode(HttpStatusCode.OK.GetHashCode()),
            ApplicationServiceStatus.NotFound => StatusCode(HttpStatusCode.NotFound.GetHashCode(), command),
            _ => BadRequest(result.Messages)
        };
    }

    protected async Task<IActionResult> Delete<TCommand, TCommandResult>(TCommand command)
        where TCommand : class, ICommand<TCommandResult>
    {
        var result = await CommandDispatcher.Send<TCommand, TCommandResult>(command);

        return result.Status switch
        {
            ApplicationServiceStatus.Ok => StatusCode(HttpStatusCode.OK.GetHashCode(), result.Data),
            ApplicationServiceStatus.NotFound => StatusCode(HttpStatusCode.NotFound.GetHashCode(), command),
            _ => BadRequest(result.Messages)
        };
    }

    protected async Task<IActionResult> Delete<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var result = await CommandDispatcher.Send(command);

        return result.Status switch
        {
            ApplicationServiceStatus.Ok => StatusCode(HttpStatusCode.OK.GetHashCode()),
            ApplicationServiceStatus.NotFound => StatusCode(HttpStatusCode.NotFound.GetHashCode(), command),
            _ => BadRequest(result.Messages)
        };
    }

    protected async Task<IActionResult> Query<TQuery, TQueryResult>(TQuery query)
        where TQuery : class, IQuery<TQueryResult>
    {
        var result = await QueryDispatcher.Execute<TQuery, TQueryResult>(query);

        if (result.Status.Equals(ApplicationServiceStatus.InvalidDomainState) ||
            result.Status.Equals(ApplicationServiceStatus.ValidationError))
        {
            return BadRequest(result.Messages);
        }
        else if (result.Status.Equals(ApplicationServiceStatus.NotFound) ||
                 result.Data == null)
        {
            return StatusCode(HttpStatusCode.NoContent.GetHashCode());
        }
        else if (result.Status.Equals(ApplicationServiceStatus.Ok))
        {
            return Ok(result.Data);
        }

        return BadRequest(result.Messages);
    }
}