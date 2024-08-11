namespace WeatherMonitoring.ServiceContracts
{
    public interface IServiceManager
    {
        ILocationService LocationService { get; }
        IWeatherService WeatherService { get; }
        IWeatherAPIService WeatherAPIService { get; }
    }
}
