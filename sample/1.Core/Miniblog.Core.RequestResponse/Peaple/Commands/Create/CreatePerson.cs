using DDDZamin.Core.RequestResponse.Commands;

namespace Miniblog.Core.RequestResponse.Peaple.Commands.Create;

public class CreatePerson:ICommand<int>
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}