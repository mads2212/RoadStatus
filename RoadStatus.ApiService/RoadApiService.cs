using System.Net;
using System.Text;
using System.Text.Json;
using RestSharp;
using RoadStatus.ApiService.Models;

namespace RoadStatus.ApiService;

public class RoadApiService : IRoadApiService
{
    private readonly string _apiKey;
    private readonly string _appId;
    
    public RoadApiService(string apiKey, string appId)
    {
        _apiKey = apiKey;
        _appId = appId;
    }
    
    public async Task<RoadApiServiceResponse> GetRoadStatusByCode(string code)
    {
        var options = new RestClientOptions($"https://api.tfl.gov.uk/");
        var client = new RestClient(options);
        var request = new RestRequest($"Road/{code}/Status?app_id={_appId}&app_key={_apiKey}");
        var response = await client.ExecuteGetAsync(request);
        Road? roadData;
        
        if (response.IsSuccessStatusCode)
        {
            roadData = JsonSerializer.Deserialize<Road[]>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })[0];
        }
        else
        {
            roadData = null;
        }

        return new RoadApiServiceResponse
        {
            IsSuccessReponse = response.IsSuccessStatusCode,
            Data = roadData,
            StatusMessage = GetStatusMessages(response.IsSuccessStatusCode, roadData, code, response.StatusCode, response.Content)
        };
    }

    private static string GetStatusMessages(bool responseIsSuccessStatusCode, Road? roadData, string code,
        HttpStatusCode responseStatusCode, string? responseContent)
    {
        var statusMessages = new StringBuilder();
        
        if (responseIsSuccessStatusCode)
        {
            statusMessages.AppendLine($"The status of the {roadData.DisplayName} is as follows");
            statusMessages.AppendLine($"Road status is {roadData.StatusSeverity}");
            statusMessages.AppendLine($"Road status Description is {roadData.StatusSeverityDescription}");
        }
        else
        {
            SetRequestFailedResponse(code, responseStatusCode, responseContent, statusMessages);
        }
        return statusMessages.ToString();
    }

    private static void SetRequestFailedResponse(string code, HttpStatusCode responseStatusCode, string? responseContent, StringBuilder statusMessages)
    {
        if (responseStatusCode == HttpStatusCode.NotFound)
            statusMessages.Append($"Road {code} is not a valid road");
        else
        {
            statusMessages.AppendLine($"An error occurred while processing your request.");
            statusMessages.AppendLine(responseContent);
        }
    }
}