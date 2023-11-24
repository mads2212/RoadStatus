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
        var request = new RestRequest($"Road/{code}?app_id= {_apiKey}&amp;app_key={_appId}");
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
            StatusMessage = GetStatusMessages(response.IsSuccessStatusCode, roadData, code)
        };
    }

    private static string GetStatusMessages(bool responseIsSuccessStatusCode, Road? roadData, string code)
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
            statusMessages.Append($"Road {code} is not a valid road");
        }
        return statusMessages.ToString();
    }
}