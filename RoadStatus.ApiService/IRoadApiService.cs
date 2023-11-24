using RoadStatus.ApiService.Models;

namespace RoadStatus.ApiService;

public interface IRoadApiService
{
    Task<RoadApiServiceResponse> GetRoadStatusByCode(string code);
}