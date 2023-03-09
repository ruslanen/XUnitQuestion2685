using XUnitQuestion2685.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/getData", () => new GetDataResult
{
    Results = new List<string> {"a", "b", "c"},
});

app.Run();

public partial class Program
{
}