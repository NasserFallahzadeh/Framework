using DDDZamin.Core.Contracts.ApplicationServices.Events;
using DDDZamin.Core.Domain.Events;

namespace DDDZamin.Core.ApplicationServices.Events;

public abstract class EventDispatcherDecorator:IEventDispatcher
{
    #region Fileds

    protected IEventDispatcher _eventDispatcher;

    public abstract int Order { get; }

    #endregion

    #region Constructors

    public EventDispatcherDecorator()
    {
        
    }

    #endregion

    #region Abstract Send Commands

    public abstract Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : class, IDomainEvent;

    public void SetEventDispatcher(IEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    #endregion
}