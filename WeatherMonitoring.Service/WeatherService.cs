using AutoMapper;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.Entities.Exceptions;
using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Service
{
    internal sealed class WeatherService : IWeatherService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public WeatherService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WeatherDto> CreateWeatherForLocationAsync(Guid locationId, WeatherForCreationDto weatherForCreation, bool trackChanges)
        {
            await CheckIfLocationExists(locationId, trackChanges);

            var weatherEntity = _mapper.Map<Weather>(weatherForCreation);

            _repository.Weather.CreateWeatherForLocation(locationId, weatherEntity);
            await _repository.SaveAsync();

            var weatherToReturn = _mapper.Map<WeatherDto>(weatherEntity);

            return weatherToReturn;
        }

        public async Task<(IEnumerable<WeatherDto> weathers, MetaData metaData)> GetWeathersAsync(Guid locationId, WeatherParameters weatherParameters, bool trackChanges)
        {
            await CheckIfLocationExists(locationId, trackChanges);

            if (weatherParameters.ValidDates && !weatherParameters.ValidPeriod)
                throw new WeatherPeriodBadRequestException();
            
            var weathersWithMetaData = await _repository.Weather.GetWeathersAsync(locationId, weatherParameters, trackChanges);
            var weathersDto = _mapper.Map<IEnumerable<WeatherDto>>(weathersWithMetaData);

            return (weathers: weathersDto, metaData: weathersWithMetaData.MetaData);
        }

        private async Task CheckIfLocationExists(Guid locationId, bool trackChanges)
        {
            var location = await _repository.Location.GetLocationAsync(locationId, trackChanges);

            if (location is null)
            {
                throw new LocationNotFoundException(locationId);
            }
        }
    }
}
