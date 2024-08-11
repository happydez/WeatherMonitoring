using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.ServiceContracts
{
    public interface IWeatherService
    {
        Task<(IEnumerable<WeatherDto> weathers, MetaData metaData)> GetWeathersAsync(Guid locationId, WeatherParameters weatherParameters, bool trackChanges);
        Task<WeatherDto> CreateWeatherForLocationAsync(Guid locationId, WeatherForCreationDto weatherForCreation, bool trackChanges);
    }
}
