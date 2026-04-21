using Microsoft.Extensions.Logging;
using System.Net;
using Zamin.Extensions.Serializers.Abstractions;
using Zamin.Extensions.Translations.Abstractions;

namespace DDDZamin.Endpoints.Web.Middlewares.ApiExceptionHandler;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly ApiExceptionOptions _options;
    private readonly IJsonSerializer _serializer;
    private readonly ITranslator _translator;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger, ApiExceptionOptions options, IJsonSerializer serializer, ITranslator translator)
    {
        _next = next;
        _logger = logger;
        _options = options;
        _serializer = serializer;
        _translator = translator;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var error = new ApiError
        {
            Id = Guid.NewGuid().ToString(),
            Status = (short)HttpStatusCode.InternalServerError,
            Title = _translator["SOME_KIND_OF_ERROR_OCCURRED_IN_THE_API"]
        };

        _options.AddResponseDetails?.Invoke(context, exception, error);

        var innerExceptionMessage = GetInnermostExceptionMessage(exception);

        var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;

        _logger.Log(level, exception, $"BADNESS!!! {innerExceptionMessage} -- " + "{ErrorId}.");

        var result = _serializer.Serialize(error);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsync(result);
    }

    private string GetInnermostExceptionMessage(Exception exception)
    {
        return exception.InnerException != null 
            ? GetInnermostExceptionMessage(exception.InnerException) 
            : exception.Message;
    }
}