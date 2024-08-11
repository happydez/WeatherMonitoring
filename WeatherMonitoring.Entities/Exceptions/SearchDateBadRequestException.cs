namespace WeatherMonitoring.Entities.Exceptions
{
    public sealed class SearchDateBadRequestException : BadRequestException
    {
        public SearchDateBadRequestException() 
            : base("Incorrect date for the search.")
        {
        }
    }
}
