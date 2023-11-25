using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RoadStatus.ApiService;

namespace RoadStatus.Tests;

public class InvalidApiConfigRoadApiServiceTests
{
    private readonly IRoadApiService _roadApiService;

    public InvalidApiConfigRoadApiServiceTests()
    {
        var apiKey = "INVALID_KEY";
        var appId = "INVALID_APP_ID";

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IRoadApiService>(_ => new RoadApiService(apiKey, appId));
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _roadApiService = serviceProvider.GetRequiredService<IRoadApiService>();
    }
    
    [Test]
    [TestCase("A2")]
    public async Task Invalid_Api_Config_Must_Return_Error_Message(string code)
    {
        var result = await _roadApiService.GetRoadStatusByCode(code);
        var expectedStatusMessage = new StringBuilder();
        expectedStatusMessage.AppendLine($"An error occurred while processing your request.");
        expectedStatusMessage.AppendLine("Invalid app_key is provided.");
        Assert.That(result.StatusMessage, Is.EqualTo(expectedStatusMessage.ToString()));
        Assert.That(result.IsSuccessReponse, Is.EqualTo(false));
    }
}