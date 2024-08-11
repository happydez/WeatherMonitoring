namespace WeatherMonitoring.Shared.RequestFeatures
{
    public class SearchParameters : RequestParameters
    {
        public SearchParameters()
        {
            limitSize = int.MaxValue;
            MaxLimitSize = int.MaxValue;
        }

        public DateOnly? Date { get; set; }
    }
}
