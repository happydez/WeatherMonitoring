using AutoMapper;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.ServiceContracts;

namespace WeatherMonitoring.Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IWeatherService> _weatherService;
        private readonly Lazy<IWeatherAPIService> _weatherAPIService;

        public ServiceManager(IRepositoryManager repositoryManager, IWeatherAPI weatherAPI, ILoggerManager logger, IMapper mapper)
        {
            _locationService = new Lazy<ILocationService>(() => new LocationService(repositoryManager, logger, mapper));
            _weatherService = new Lazy<IWeatherService>(() => new WeatherService(repositoryManager, logger, mapper));
            _weatherAPIService = new Lazy<IWeatherAPIService>(() => new WeatherAPIService(weatherAPI, logger, mapper));
        }

        public ILocationService LocationService => _locationService.Value;

        public IWeatherService WeatherService => _weatherService.Value;

        public IWeatherAPIService WeatherAPIService => _weatherAPIService.Value;
    }
}
