using DDDZamin.Core.RequestResponse.Commands;

namespace DDDZamin.Core.Contracts.ApplicationServices.Commands;

/// <summary>
/// تعریف ساختار برای مدیریت دستورات. پیاده سازی الگوی Mediator
/// از این الگو جهت کاهش پیچیدیگی صدازدن دستورات استفاده می‌شود
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// یک دستور از نوع ICommand را دریافت کرده و پیاده سازی مناسب جهت مدیریت این دستور را یافته و کار را برای ادامه پردازش به آن پیاده سازی تحویل می‌شود.
    /// </summary>
    /// <typeparam name="TCommand">نوع دستور را تعیین می‌کند</typeparam>
    /// <param name="command">نام دستور</param>
    /// <returns></returns>
    Task<CommandResult> Send<TCommand>(TCommand command) where TCommand : class, ICommand;

    Task<CommandResult<TData>> Send<TCommand, TData>(TCommand command) where TCommand : class, ICommand<TData>;
}