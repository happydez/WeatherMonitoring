using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Contracts
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<PagedList<Location>> GetAllLocationsAsync(LocationParameters locationParameters, bool trackChanges);
        Task<Location> GetLocationAsync(Guid locationId, bool trackChanges);
        Task<Location> GetLocationByNameAsync(string fullName,  bool trackChanges);
        void CreateLocation(Location location);
        void DeleteLocation(Location location);
    }
}
