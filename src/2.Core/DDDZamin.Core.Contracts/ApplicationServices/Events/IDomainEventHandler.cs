using DDDZamin.Core.Domain.Events;

namespace DDDZamin.Core.Contracts.ApplicationServices.Events;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent:IDomainEvent
{
    Task Handle(TDomainEvent Event);
}