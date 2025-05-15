using DDDZamin.Core.Contracts.Data.Commands;
using Miniblog.Core.Domain.People.Entities;

namespace Miniblog.Core.Contracts.People.Commands;

public interface IPersonCommandRepository:ICommandRepository<Person,int>
{
    
}