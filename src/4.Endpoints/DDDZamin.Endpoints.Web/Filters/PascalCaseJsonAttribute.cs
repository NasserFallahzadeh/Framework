using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DDDZamin.Endpoints.Web.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class PascalCaseJsonAttribute:Attribute
{
    
}

public class PascalCaseJsonFilter : IAsyncResultFilter
{
    public async Task OnRsultExecutionAsync(ResultExecutingContext context, RsultExecutionDelegate next)
    {
        var hasPascalCaseAttr = context.ActionDescriptor.EndpointMetadata.OfType<PascalCaseJsonAttribute>().Any();

        if (hasPascalCaseAttr &&
            context.Result is ObjectResult objectResult)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            objectResult.Formatters.Clear();
            objectResult.Formatters.Add(new SystemTextJsonOutputFormatter(options));

            await next();
        }
    }
}