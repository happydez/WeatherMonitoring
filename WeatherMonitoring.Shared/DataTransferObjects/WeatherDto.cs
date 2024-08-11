namespace WeatherMonitoring.Shared.DataTransferObjects
{
    public record WeatherDto
    {
        public int ConditionCode { get; init; }
        public string? ConditionText { get; init; }
        public double TemperatureCelsius { get; init; }
        public int Humidity { get; init; }
        public double PressureIn { get; init; }
        public double WindSpeedKph { get; init; }
        public long LastUpdatedEpoch { get; init; }
        public DateTime LastUpdated { get; init; }
    }
}
