using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoadStatus.ApiService;

namespace RoadStatus.Tests;

public class Tests
{
    private readonly IRoadApiService _roadApiService;

    public Tests()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", true, true);
        
        var config = builder.Build();
        
        var apiKey = config["API:Key"];
        var appId = config["API:AppId"];

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IRoadApiService>(_ => new RoadApiService(apiKey, appId));
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _roadApiService = serviceProvider.GetRequiredService<IRoadApiService>();
    }

    [Test]
    [TestCase("A2")]
    public async Task Valid_Code_Must_Return_IsSuccessResponse_In_Response(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        Assert.That(result.IsSuccessReponse);
    }

    [TestCase("A2")]
    public async Task Valid_Code_Must_Return_Road_Data_In_Response(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        Assert.That(result.Data, Is.Not.Null);
    }
    
    [TestCase("893ka")]
    public async Task Invalid_Code_Must_Return_IsSuccessResponse_False_In_Response(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        Assert.That(result.IsSuccessReponse, Is.False);
    }
    
    [TestCase("893ka")]
    public async Task Invalid_Code_Must_Return_Null_Road_Data_In_Response(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        Assert.That(result.Data, Is.Null);
    }

    [TestCase("A2")]
    public async Task Valid_Code_Must_Return_Success_Status_Message_In_Response(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        Assert.That(result.StatusMessage, Is.Not.Null);
    }
    
    [TestCase("A2")]
    public async Task StatusMessage_Should_Be_Correct_For_Valid_Code(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        var roadData = result.Data;
        var expectedStatusMessage = new StringBuilder();
        expectedStatusMessage.AppendLine($"The status of the {roadData.DisplayName} is as follows");
        expectedStatusMessage.AppendLine($"Road status is {roadData.StatusSeverity}");
        expectedStatusMessage.AppendLine($"Road status Description is {roadData.StatusSeverityDescription}");
        Assert.That(result.StatusMessage, Is.EqualTo(expectedStatusMessage.ToString()));
    }
    
    [TestCase("AAEF12")]
    public async Task StatusMessage_Should_Be_Correct_For_Invalid_Code(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        var expectedStatusMessage = new StringBuilder();
        expectedStatusMessage.Append($"Road {code} is not a valid road");
        Assert.That(result.StatusMessage, Is.EqualTo(expectedStatusMessage.ToString()));
    }
}