namespace WeatherMonitoring.Shared.RequestFeatures
{
    public class MetaData
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious => Offset > 0;
        public bool HasNext => Offset < TotalCount - 1;
    }
}
