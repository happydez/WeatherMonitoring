namespace WeatherMonitoring.Entities.Exceptions
{
    public sealed class LocationCollectionBadRequest : BadRequestException
    {
        public LocationCollectionBadRequest()
            : base("Location collection sent from a client is null.")
        {
            
        }
    }
}
