using AutoMapper;
using System.Timers;
using MonitoringTimer = System.Timers.Timer;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.Shared.RequestFeatures;
using WeatherMonitoring.Shared.DataTransferObjects;
using Microsoft.Extensions.Configuration;

namespace WeatherMonitoring.MonitoringService
{
    public static class MonitoringManager
    {
        private static MonitoringTimer _timer;

        public static void StartMonitoring(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            double.TryParse(configuration.GetSection("MonitoringManager:Interval").Value, out double interval);

            _timer = new MonitoringTimer(interval);
            _timer.Elapsed += (sender, e) => OnTimedEvent(serviceProvider, sender, e);
            _timer.AutoReset = true;
            _timer.Enabled = true;

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                Start(scopedServiceProvider, null, null).Wait();
            }
        }

        private static async void OnTimedEvent(IServiceProvider serviceProvider, object sender, ElapsedEventArgs e)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;

                await Start(scopedServiceProvider, sender, e);
            }
        }

        private static async Task Start(IServiceProvider serviceProvider, object sender, ElapsedEventArgs e)
        {
            var service = serviceProvider.GetRequiredService<IServiceManager>();
            var logger = serviceProvider.GetRequiredService<ILoggerManager>();
            var mapper = serviceProvider.GetRequiredService<IMapper>();

            var locatonParams = new LocationParameters()
            {
                Included = true,
                Active = true,
            };

            var locationsWithMetaData = await service.LocationService
                .GetAllLocationsAsync(locatonParams, trackChanges: false);

            foreach (var location in locationsWithMetaData.locations)
            {
                var q = string.Join(',', location.Name, location.Region, location.Country);
                var apiResponse = await service.WeatherAPIService.GetWeatherDataAsync(q);

                var weatherDto = mapper.Map<WeatherForCreationDto>(apiResponse);
                await service.WeatherService.CreateWeatherForLocationAsync(location.Id, weatherDto, trackChanges: false);

                logger.LogInfo($"Up-to-date information for the included and active {q} location has been received.");
            }
        }
    }
}
