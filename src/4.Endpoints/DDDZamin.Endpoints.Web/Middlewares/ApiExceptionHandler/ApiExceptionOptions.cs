using Microsoft.Extensions.Logging;

namespace DDDZamin.Endpoints.Web.Middlewares.ApiExceptionHandler;

public class ApiExceptionOptions
{
    public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }

    public Func<Exception,LogLevel> DetermineLogLevel { get; set; }
}