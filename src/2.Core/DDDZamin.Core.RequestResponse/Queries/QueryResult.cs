using DDDZamin.Core.RequestResponse.Common;

namespace DDDZamin.Core.RequestResponse.Queries;

public sealed class QueryResult<TData> : ApplicationServiceResult
{
    public TData? _data;

    public TData? Data => _data;
}