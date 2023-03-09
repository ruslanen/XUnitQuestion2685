using System.Net;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;
using XUnitQuestion2685.Api;

namespace XUnitQuestion2685.Tests;

public class Tests
{
    [Theory]
    [TestFolderData("Tests")]
    public Task Test(TestData testData) => RunTest(testData);
    
    private async Task RunTest(TestData testData)
    {
        // Arrange
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Testing"));

        // Act
        var response = await application.CreateClient().GetAsync("api/getData");

            // Assert
        response.StatusCode.Should().Be(testData.StatusCode);
        if (testData.StatusCode != HttpStatusCode.OK)
            return;

        var executionResult =
            JsonConvert.DeserializeObject<GetDataResult>(await response.Content.ReadAsStringAsync());

        executionResult.Results.Should().NotBeNull();
        executionResult.Results.Should().BeEquivalentTo(testData.Expected);
    }
}

public class TestFolderDataAttribute : DataAttribute
{
    private readonly string _folderPath;

    public TestFolderDataAttribute(string folderPath)
    {
        _folderPath = folderPath;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var path = Path.IsPathRooted(_folderPath)
            ? _folderPath
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), _folderPath);

        foreach (var filePath in GetTestDataFiles(path))
        {
            var json = File.ReadAllText(filePath);

            var testData = JsonConvert.DeserializeObject<TestData>(json);

            testData.TestDescription = filePath.Replace(path, "").TrimStart('\\');

            yield return new object[] { testData };
        }
    }
    
    private List<string> GetTestDataFiles(string path)
    {
        var files = new List<string>();

        foreach (var file in Directory.GetFiles(path, "*.json"))
        {
            files.Add(file);
        }

        foreach (var directory in Directory.GetDirectories(path))
        {
            files.AddRange(GetTestDataFiles(directory));
        }

        return files;
    }
}