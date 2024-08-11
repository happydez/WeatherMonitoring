using AutoMapper;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.Entities.WeatherAPIModels;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Service
{
    public class WeatherAPIService : IWeatherAPIService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IWeatherAPI _weatherAPI;

        public WeatherAPIService(IWeatherAPI weatherAPI, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _weatherAPI = weatherAPI;
        }

        public async Task<APIResponse> GetWeatherDataAsync(string q)
        {
            var apiResponse = await _weatherAPI.GetWeatherDataAsync(q);

            return apiResponse;
        }

        public async Task<(IEnumerable<APIResponse> apiResponse, MetaData metaData)> GetWeatherHistoryAsync(string q, SearchParameters searchParameters)
        {
            var apiResponseWithMetaData = await _weatherAPI.GetWeatherHistoryAsync(q, searchParameters);
            var apiResponse = _mapper.Map<IEnumerable<APIResponse>>(apiResponseWithMetaData);

            return (apiResponse, apiResponseWithMetaData.MetaData);
        }

        public async Task<IEnumerable<LocationForManipulationDto>> SearchLocationsAsync(string q)
        {
            var apiResponse = await _weatherAPI.SearchLocationsAsync(q);
            var locationsDto = _mapper.Map<IEnumerable<LocationForManipulationDto>>(apiResponse);

            return locationsDto;
        }
    }
}
