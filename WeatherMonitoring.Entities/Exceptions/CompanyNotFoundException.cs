namespace WeatherMonitoring.Entities.Exceptions
{
    public class LocationNotFoundException : NotFoundException
    {
        public LocationNotFoundException(Guid locationId)
            : base($"The location with id: {locationId} doesn't exist in the database.")
        {

        }
    }
}
