using DDDZamin.Core.RequestResponse.Queries;

namespace DDDZamin.Core.Contracts.ApplicationServices.Queries;

/// <summary>
/// تعریف ساختار الگوی Mediator جهت اتصال ساده کوئری‌ها به هندلرها
/// </summary>
public interface IQueryDispatcher
{
    Task<QueryResult<TData>> Execute<TQuery, TData>(TQuery query) where TQuery : class, IQuery<TData>;
}