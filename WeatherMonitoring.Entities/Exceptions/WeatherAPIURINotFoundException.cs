namespace WeatherMonitoring.Entities.Exceptions
{
    public sealed class WeatherAPIURINotFoundException : NotFoundException
    {
        public WeatherAPIURINotFoundException()
            : base("WeatherAPI URL not found in the configuration.")
        {
        }
    }
}
