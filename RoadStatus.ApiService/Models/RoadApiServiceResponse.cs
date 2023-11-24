namespace RoadStatus.ApiService.Models;

public class RoadApiServiceResponse
{
    public bool IsSuccessReponse { get; set; }

    public Road Data { get; set; }
    
    public string StatusMessage { get; set; }
}