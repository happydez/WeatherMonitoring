using WeatherMonitoring.Entities.WeatherAPIModels;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Contracts
{
    public interface IWeatherAPI
    {
        Task<APIResponse> GetWeatherDataAsync(string q);
        Task<IEnumerable<APIResponse>> SearchLocationsAsync(string q);
        Task<PagedList<APIResponse>> GetWeatherHistoryAsync(string q, SearchParameters searchParameters);
    }
}
