using System.Net;
using Xunit.Abstractions;
using XUnitQuestion2685.Api;

namespace XUnitQuestion2685.Tests;

public class TestData : IXunitSerializable
{
    public string TestDescription { get; set; } = "";

    public string Query { get; set; } = "";

    public List<string> Expected { get; set; } = new();

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public override string ToString() => TestDescription;
    
    public void Deserialize(IXunitSerializationInfo info)
    {
        //throw new NotImplementedException();
    }
    
    public void Serialize(IXunitSerializationInfo info)
    {
        //throw new NotImplementedException();
    }
}