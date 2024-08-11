namespace WeatherMonitoring.Shared.RequestFeatures
{
    public abstract class RequestParameters
    {
        protected int limitSize = 20;
        protected int MaxLimitSize { get; init; } = 50;
        
        public int Offset { get; set; } = 0;

        public int Limit
        {
            get
            {
                return limitSize;
            }

            set
            {
                limitSize = (value > MaxLimitSize) ? MaxLimitSize : value;
            }
        }
    }
}
