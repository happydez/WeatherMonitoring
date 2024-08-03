using WeatherMonitoring.Api.Configuration;
using WeatherMonitoring.Services.Locations;
using WeatherMonitoring.Services.Logger;
using WeatherMonitoring.Services.Settings;
using WeatherMonitoring.Services.WeatherApi;
using WeatherMonitoring.Services.Weathers;

namespace WeatherMonitoring.Api
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services
                .AddMainSettings()
                .AddLogSettings()
                .AddAppLogger()
                .AddWeatherApi()
                .AddLocationService()
                .AddWeatherService()
                .AddLocationService()
                ;

            return services;
        }
    }
}
