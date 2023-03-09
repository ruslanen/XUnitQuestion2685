namespace XUnitQuestion2685.Api;

public sealed class GetDataResult
{
    public List<string> Results { get; set; } = default!;
}

public class QueryRequest
{
    public string Query { get; set; } = default!;
}