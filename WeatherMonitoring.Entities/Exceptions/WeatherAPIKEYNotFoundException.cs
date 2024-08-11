namespace WeatherMonitoring.Entities.Exceptions
{
    public sealed class WeatherAPIKEYNotFoundException : NotFoundException
    {
        public WeatherAPIKEYNotFoundException()
            : base("WeatherAPI KEY not found in the configuration.")
        {
        }
    }
}
