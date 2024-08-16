using AutoMapper;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.Entities.Exceptions;
using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Service
{
    internal sealed class LocationService : ILocationService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public LocationService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<LocationDto> CreateLocationAsync(LocationForCreationDto location)
        {
            var locationFullName = string.Join(',', location.Name, location.Region, location.Country);
            var locationEntity = await _repository.Location.GetLocationByNameAsync(locationFullName, trackChanges: true);

            if (locationEntity is null)
            {
                locationEntity = _mapper.Map<Location>(location);
                _repository.Location.CreateLocation(locationEntity);
            }
            else
            {
                _mapper.Map(location, locationEntity);
            }

            await _repository.SaveAsync();

            var locationToReturn = _mapper.Map<LocationDto>(locationEntity);

            return locationToReturn;
        }

        public async Task<(IEnumerable<LocationDto> locations, string ids)> CreateLocationCollectionAsync(IEnumerable<LocationForCreationDto> locationCollection)
        {
            if (locationCollection is null)
            {
                throw new LocationCollectionBadRequest();
            }

            var locationEntities = _mapper.Map<IEnumerable<Location>>(locationCollection);

            foreach (var location in locationEntities)
            {
                _repository.Location.CreateLocation(location);
            }

            await _repository.SaveAsync();

            var locationCollectionToReturn = _mapper.Map<IEnumerable<LocationDto>>(locationEntities);
            var ids = string.Join(",", locationCollectionToReturn.Select(l => l.Id));

            return (locations: locationCollectionToReturn, ids: ids);
        }

        public async Task DeleteLocationAsync(Guid locationId, bool trackChanges)
        {
            var location = await GetLocationAndCheckIfItExists(locationId, trackChanges);

            _repository.Location.DeleteLocation(location);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<LocationDto> locations, MetaData metaData)> GetAllLocationsAsync(LocationParameters locationParameter, bool trackChanges)
        {
            var locationsWithMetaData = await _repository.Location.GetAllLocationsAsync(locationParameter, trackChanges);
            var locationsDto = _mapper.Map<IEnumerable<LocationDto>>(locationsWithMetaData);

            return (locationsDto, locationsWithMetaData.MetaData);
        }

        public async Task<IEnumerable<LocationDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
            {
                throw new IdParametersBadRequestException();
            }

            var locationEntities = await _repository.Location.GetByIdsAsync(ids, trackChanges);

            if (ids.Count() != locationEntities.Count())
            {
                throw new CollectionByIdsBadRequestException();
            }

            var locationsToReturn = _mapper.Map<IEnumerable<LocationDto>>(locationEntities);

            return locationsToReturn;
        }

        public async Task<LocationDto> GetLocationAsync(Guid locationId, bool trackChanges)
        {
            var location = await GetLocationAndCheckIfItExists(locationId, trackChanges);

            var locationDto = _mapper.Map<LocationDto>(location);

            return locationDto;
        }

        public async Task<LocationDto> GetLocationByNameAsync(string fullName, bool trackChanges)
        {
            var location = await _repository.Location.GetLocationByNameAsync(fullName, trackChanges);

            if (location is null)
            {
                throw new LocationNotFoundByNameException(fullName);
            }

            var locationDto = _mapper.Map<LocationDto>(location);

            return locationDto;
        }

        public async Task<(LocationForUpdateDto locationToPatch, Location locationEntity)> GetLocationForPatchAsync(Guid locationId, bool trackChanges)
        {
            var location = await GetLocationAndCheckIfItExists(locationId, trackChanges);

            var locationToPatch = _mapper.Map<LocationForUpdateDto>(location);

            return (locationToPatch, location);
        }

        public async Task SaveChangesForPatchAsync(LocationForUpdateDto locationToPatch, Location location)
        {
            _mapper.Map(locationToPatch, location);
            await _repository.SaveAsync();
        }

        public async Task UpdateLocationAsync(Guid locationId, LocationForUpdateDto locationForUpdate, bool trackChanges)
        {
            var locationEntity = await GetLocationAndCheckIfItExists(locationId, trackChanges);

            _mapper.Map(locationForUpdate, locationEntity);
            await _repository.SaveAsync();
        }

        private async Task<Location> GetLocationAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var location = await _repository.Location.GetLocationAsync(id, trackChanges);

            if (location is null)
            {
                throw new LocationNotFoundException(id);
            }

            return location;
        }
    }
}
