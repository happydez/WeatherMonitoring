namespace WeatherMonitoring.Entities.Exceptions
{
    public sealed class LocationNotFoundByNameException : NotFoundException
    {
        public LocationNotFoundByNameException(string fullName)
            : base($"The location with full name: {fullName} doesn't exist in the database.")
        {
            
        }
    }
}
