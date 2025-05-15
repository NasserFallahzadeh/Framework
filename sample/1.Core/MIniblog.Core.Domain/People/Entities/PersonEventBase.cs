using DDDZamin.Core.Domain.Entities;
using DDDZamin.Core.Domain.Exceptions;
using Miniblog.Core.Domain.People.DomainEvents;
using Miniblog.Core.Domain.People.ValueObjects;
using Miniblog.Core.Domain.Resources;

namespace Miniblog.Core.Domain.People.Entities;

public class PersonEventBase : AggregateRoot<int>
{
    #region Properties

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; set; }

    #endregion

    public PersonEventBase(int id, string firstName, string lastName)
    {
        Apply(new PersonCreated(id, BusinessId.Value, firstName, lastName));
    }

    private void On(PersonCreated personCreated)
    {
        if (personCreated.Id < 1)
        {
            throw new InvalidEntityStateException(MessagePattern.IdValidationMessage);
        }

        Id = personCreated.Id;
        FirstName = personCreated.FirstName;
        LastName = personCreated.LastName;
        BusinessId = personCreated.BusinessId;
    }
}