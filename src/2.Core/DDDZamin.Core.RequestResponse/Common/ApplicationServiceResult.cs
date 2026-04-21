namespace DDDZamin.Core.RequestResponse.Common;

public abstract class ApplicationServiceResult:IApplicationServiceResult
{
    protected readonly List<string> _messages = [];

    public IEnumerable<string> Messages => _messages;

    public ApplicationServiceStatus Status { get; set; }

    public void AddMessage(string error) => _messages.Add(error);

    public void AddMessages(List<string> errors) => _messages.AddRange(errors);

    public void ClearMessages() => _messages.Clear();
}