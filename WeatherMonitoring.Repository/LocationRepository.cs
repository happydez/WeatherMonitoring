using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Repository
{
    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateLocation(Location location)
        {
            Create(location);
        }

        public void DeleteLocation(Location location)
        {
            Delete(location);
        }

        public async Task<PagedList<Location>> GetAllLocationsAsync(LocationParameters locationParameters, bool trackChanges)
        {
            var locations = await FindAll(trackChanges)
                .ToListAsync();

            if (locationParameters.Included != null)
                locations = locations.Where(l => l.Included == locationParameters.Included.Value).ToList();

            if (locationParameters.Active != null)
                locations = locations.Where(l => l.Active == locationParameters.Active.Value).ToList();

            return PagedList<Location>.ToPagedList(locations, locationParameters.Offset, locationParameters.Limit);
        }

        public async Task<IEnumerable<Location>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id), trackChanges)
                .ToListAsync();
        }

        public async Task<Location> GetLocationAsync(Guid locationId, bool trackChanges)
        {
            return (await FindByCondition(l => l.Id.Equals(locationId), trackChanges)
                .SingleOrDefaultAsync())!;
        }
    }
}
