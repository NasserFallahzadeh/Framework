using DDDZamin.Core.RequestResponse.Queries;

namespace DDDZamin.Core.Contracts.ApplicationServices.Queries;

/// <summary>
/// تعریف ساختار مورد نیاز جهت پردازش یک کوئری
/// </summary>
/// <typeparam name="TQuery">نوع کوئری و پارامترهای ورودی را تعیین می‌کند</typeparam>
/// <typeparam name="TData">نوع داده‌های بازگشتی را تعیین می‌کند</typeparam>
public interface IQueryHandler<TQuery,TData> where TQuery:class,IQuery<TData>
{
    Task<QueryResult<TData>> Handle(TQuery request);
}