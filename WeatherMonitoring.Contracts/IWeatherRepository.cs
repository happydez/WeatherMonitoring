using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Contracts
{
    public interface IWeatherRepository
    {
        Task<PagedList<Weather>> GetWeathersAsync(Guid locationId, WeatherParameters weatherParameters, bool trackChanges);
        void CreateWeatherForLocation(Guid locationId, Weather weather);
    }
}
