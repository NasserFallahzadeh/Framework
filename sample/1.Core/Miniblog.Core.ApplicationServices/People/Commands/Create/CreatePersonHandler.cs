using DDDZamin.Core.ApplicationServices.Commands;
using DDDZamin.Core.RequestResponse.Commands;
using DDDZamin.Utilities;
using Miniblog.Core.Contracts.People.Commands;
using Miniblog.Core.Domain.People.Entities;
using Miniblog.Core.RequestResponse.Peaple.Commands.Create;

namespace Miniblog.Core.ApplicationServices.People.Commands.Create;

public class CreatePersonHandler:CommandHandler<CreatePerson,int>
{
    private readonly IPersonCommandRepository _personCommandRepository;
    public CreatePersonHandler(ZaminServices zaminServices, IPersonCommandRepository personCommandRepository) : base(zaminServices)
    {
        _personCommandRepository = personCommandRepository;
    }

    public override async Task<CommandResult<int>> Handle(CreatePerson command)
    {
        var person = new Person(command.Id, command.FirstName, command.LastName);

        _personCommandRepository.Insert(person);

        await _personCommandRepository.CommitAsync();

        return await OkAsync(person.Id);
    }
}