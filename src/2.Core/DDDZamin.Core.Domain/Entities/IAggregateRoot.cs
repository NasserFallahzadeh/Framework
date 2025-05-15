using DDDZamin.Core.Domain.Events;

namespace DDDZamin.Core.Domain.Entities;

public interface IAggregateRoot
{
    void ClearEvents();

    IEnumerable<IDomainEvent> GetEvents();
}