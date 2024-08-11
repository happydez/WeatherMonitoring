namespace WeatherMonitoring.Shared.RequestFeatures
{
    public class LocationParameters : RequestParameters
    {
        public LocationParameters()
        {
            limitSize = int.MaxValue;
            MaxLimitSize = int.MaxValue;
        }

        public bool? Active { get; set; }
        public bool? Included { get; set; }
    }
}
