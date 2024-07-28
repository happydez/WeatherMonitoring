using Microsoft.Extensions.DependencyInjection;
using WeatherMonitoring.Services.Locations;
using System.Timers;
using MonitoringTimer = System.Timers.Timer;

namespace WeatherMonitoring.Serivces.WeatherMonitoring;

public static class WeatherMonitoring
{
    private static MonitoringTimer _timer;
    private static ILocationService _locationService;

    public static void Initialize(IServiceProvider provider)
    {
        _locationService = provider.GetRequiredService<ILocationService>();

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
        await Collect();
    }

    private static async Task Collect()
    {
        await Task.Run(async () =>
        {
            await Task.Delay(500);
        });
    }
}