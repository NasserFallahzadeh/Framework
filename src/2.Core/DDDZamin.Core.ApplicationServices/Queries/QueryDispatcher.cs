using System.Diagnostics;
using DDDZamin.Core.Contracts.ApplicationServices.Queries;
using DDDZamin.Core.RequestResponse.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zamin.Extensions.Logger.Abstractions;

namespace DDDZamin.Core.ApplicationServices.Queries;

public class QueryDispatcher:IQueryDispatcher
{
    #region Fields

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueryDispatcher> _logger;
    private readonly Stopwatch _stopwatch;

    #endregion

    #region Costructors

    public QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger, Stopwatch stopwatch)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _stopwatch = stopwatch;
    }

    #endregion

    #region QueryDispatcher

    public Task<QueryResult<TData>> Execute<TQuery, TData>(TQuery query) where TQuery : class, IQuery<TData>
    {
        _stopwatch.Start();
        try
        {
            _logger.LogDebug("Routing query of type {QueryType} With value {Query} Start at {StartDateTime}",
                query.GetType(), query, DateTime.Now);

            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TData>>();

            return handler.Handle(query);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "There is not suitable handler for {QueryType} Routing failed at {StartDateTime}.",
                query.GetType(), DateTime.Now);
            throw;
        }
        finally
        {
            _stopwatch.Stop();
            _logger.LogInformation(ZaminEventId.PerformanceMeasurement,
                "Processing the {QueryType} query tooks {Milliseconds} Milliseconds", query.GetType(),
                _stopwatch.ElapsedMilliseconds);
        }
    }

    #endregion
}