using DDDZamin.Core.RequestResponse.Commands;

namespace Miniblog.Core.RequestResponse.Peaple.Commands.ChangeName;

public class ChangePersonName : ICommand<int>
{
    public int Id { get; set; }

    public string FirstName { get; set; }
}