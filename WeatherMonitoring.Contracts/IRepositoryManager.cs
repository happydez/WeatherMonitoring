namespace WeatherMonitoring.Contracts
{
    public interface IRepositoryManager
    {
        ILocationRepository Location { get; }
        IWeatherRepository Weather { get; }
        Task SaveAsync();
    }
}
