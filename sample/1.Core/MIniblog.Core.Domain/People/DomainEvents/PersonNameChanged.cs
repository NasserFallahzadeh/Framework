using DDDZamin.Core.Domain.Events;

namespace Miniblog.Core.Domain.People.DomainEvents;

public record PersonNameChanged(int Id, string FirstName) : IDomainEvent;