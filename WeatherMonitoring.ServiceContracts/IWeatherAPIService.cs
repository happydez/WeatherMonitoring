using WeatherMonitoring.Entities.WeatherAPIModels;
using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.ServiceContracts
{
    public interface IWeatherAPIService
    {
        Task<IEnumerable<LocationForManipulationDto>> SearchLocationsAsync(string q);
        Task<APIResponse> GetWeatherDataAsync(string q);
        Task<(IEnumerable<APIResponse> apiResponse, MetaData metaData)> GetWeatherHistoryAsync(string q, SearchParameters searchParameters);
    }
}
