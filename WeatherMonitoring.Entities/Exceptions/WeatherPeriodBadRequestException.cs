namespace WeatherMonitoring.Entities.Exceptions
{
    public sealed class WeatherPeriodBadRequestException : BadRequestException
    {
        public WeatherPeriodBadRequestException()
            : base("The Date1 and Date2 parameters must be passed together and Date2 must be greater than or equal to Date1")
        {
            
        }
    }
}
