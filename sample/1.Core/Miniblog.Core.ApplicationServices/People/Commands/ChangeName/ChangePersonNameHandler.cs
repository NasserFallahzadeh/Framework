using DDDZamin.Core.ApplicationServices.Commands;
using DDDZamin.Core.RequestResponse.Commands;
using DDDZamin.Utilities;
using Miniblog.Core.Contracts.People.Commands;
using Miniblog.Core.RequestResponse.Peaple.Commands.ChangeName;

namespace Miniblog.Core.ApplicationServices.People.Commands.ChangeName;

public class ChangePersonNameHandler:CommandHandler<ChangePersonName,int>
{
    private readonly IPersonCommandRepository _personCommandRepository;

    public ChangePersonNameHandler(ZaminServices zaminServices, IPersonCommandRepository personCommandRepository) : base(zaminServices)
    {
        _personCommandRepository = personCommandRepository;
    }

    public override async Task<CommandResult<int>> Handle(ChangePersonName command)
    {
        var person = _personCommandRepository.GetGraph(command.Id);
        person.ChangeFirstName(command.FirstName);

        await _personCommandRepository.CommitAsync();

        return await OkAsync(person.Id);
    }
}