using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Repository
{
    public class WeatherRepository : RepositoryBase<Weather>, IWeatherRepository
    {
        public WeatherRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateWeatherForLocation(Guid locationId, Weather weather)
        {
            weather.LocationId = locationId;
            Create(weather);
        }

        public async Task<PagedList<Weather>> GetWeathersAsync(Guid locationId, WeatherParameters weatherParameters, bool trackChanges)
        {
            var weathers = await FindByCondition(w => w.LocationId.Equals(locationId), trackChanges)
                .OrderBy(w => w.LastUpdated)
                .ToListAsync();

            if (weatherParameters.ValidPeriod)
                weathers = weathers.Where(w => 
                    w.LastUpdated >= weatherParameters.Date1 
                    && w.LastUpdated <= weatherParameters.Date2).ToList();

            if (weatherParameters.Desc)
                weathers.Reverse();

            return PagedList<Weather>.ToPagedList(weathers, weatherParameters.Offset, weatherParameters.Limit);
        }
    }
}
