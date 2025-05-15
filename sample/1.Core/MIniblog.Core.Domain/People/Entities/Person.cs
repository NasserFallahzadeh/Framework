using DDDZamin.Core.Domain.Entities;
using DDDZamin.Core.Domain.Exceptions;
using Miniblog.Core.Domain.People.DomainEvents;
using Miniblog.Core.Domain.People.ValueObjects;
using Miniblog.Core.Domain.Resources;

namespace Miniblog.Core.Domain.People.Entities;

public class Person : AggregateRoot<int>
{
    #region Properties

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; set; }

    #endregion

    public Person(int id, string firstName, string lastName)
    {
        if (id < 1)
        {
            throw new InvalidEntityStateException(MessagePattern.IdValidationMessage);
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        AddEvent(new PersonCreated(id, BusinessId.Value, firstName, lastName));
    }

    public void ChangeFirstName(string firstName)
    {
        FirstName = firstName;

        AddEvent(new PersonNameChanged(Id,firstName));
    }
}