using Microsoft.Extensions.DependencyInjection;
using System.Timers;
using MonitoringTimer = System.Timers.Timer;
using WeatherMonitoring.Services.Locations;
using WeatherMonitoring.Services.WeatherApi;
using WeatherMonitoring.Services.Weathers;
using WeatherMonitoring.Services.Logger;
using AutoMapper;

namespace WeatherMonitoring.Services.WeatherMonitoring;

public static class WeatherMonitoring
{
    private static MonitoringTimer _timer;
    private static ILocationService _locationService;
    private static IWeatherApi _weatherApi;
    private static IWeatherService _weatherService;
    private static IMapper _mapper;
    private static IAppLogger _logger;

    public static void Execute(IServiceProvider provider)
    {
        _locationService = provider.GetRequiredService<ILocationService>();
        _weatherApi = provider.GetRequiredService<IWeatherApi>();
        _weatherService = provider.GetRequiredService<IWeatherService>();
        _mapper = provider.GetRequiredService<IMapper>();
        _logger = provider.GetRequiredService<IAppLogger>();

        var interval = new TimeSpan(1, 0, 0).TotalMilliseconds;
        _timer = new MonitoringTimer(interval);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;

        Start(null, null).Wait();
    }

    private static async void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        await Start(sender, e);
    }

    private static async Task Start(object sender, ElapsedEventArgs e)
    {
        var locations = await _locationService.GetAll(included: true, active: true, null, null);

        foreach (var location in locations)
        {
            var waq = WeatherApiQuery.CreateBuilder(location.Name)
                        .WithRegion(location.Region).WithRegion(location.Country).Build();

            var result = await _weatherApi.GetWeatherDataAsync(waq);

            var model = _mapper.Map<CreateWeatherModel>(result);
            model.LocationId = location.Id;

            await _weatherService.Create(model);

            _logger.Information($"Up-to-date information for the included and active {waq} location has been received.");
        }
    }
}