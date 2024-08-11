using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.ServiceContracts
{
    public interface ILocationService
    {
        Task<(IEnumerable<LocationDto> locations, MetaData metaData)> GetAllLocationsAsync(LocationParameters locationParameters, bool trackChanges);
        Task<LocationDto> GetLocationAsync(Guid locationId, bool trackChanges);
        Task<LocationDto> CreateLocationAsync(LocationForCreationDto location);
        Task<IEnumerable<LocationDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<LocationDto> locations, string ids)> CreateLocationCollectionAsync(IEnumerable<LocationForCreationDto> locationCollection);
        Task DeleteLocationAsync(Guid locationId, bool trackChanges);
        Task UpdateLocationAsync(Guid locationId, LocationForUpdateDto locationForUpdate, bool trackChanges);
        Task<(LocationForUpdateDto locationToPatch, Location locationEntity)> GetLocationForPatchAsync(Guid locationId, bool trackChanges);
        Task SaveChangesForPatchAsync(LocationForUpdateDto locationToPatch, Location location);
    }
}
