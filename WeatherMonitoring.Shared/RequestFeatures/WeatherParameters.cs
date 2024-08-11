namespace WeatherMonitoring.Shared.RequestFeatures
{
    public class WeatherParameters : RequestParameters
    {
        public WeatherParameters()
        {
            limitSize = int.MaxValue;
            MaxLimitSize = int.MaxValue;
        }

        public bool Desc { get; set; } = false;

        public DateTime? Date1 { get; set; }

        public DateTime? Date2 { get; set; }

        public bool ValidDates => Date1 != null && Date2 != null;
        public bool ValidPeriod => ValidDates && Date2!.Value >= Date1!.Value;
    }
}
