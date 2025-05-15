using DDDZamin.Core.Domain.Events;

namespace Miniblog.Core.Domain.People.DomainEvents;

public record PersonCreated(int Id,Guid BusinessId,string FirstName,string LastName):IDomainEvent;