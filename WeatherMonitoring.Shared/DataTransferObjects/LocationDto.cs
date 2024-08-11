namespace WeatherMonitoring.Shared.DataTransferObjects
{
    public record LocationDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Region { get; init; }
        public string? Country { get; init; }
        public double Lat { get; init; }
        public double Lon { get; init; }
        public string? TzId { get; init; }
        public bool Active { get; init; }
        public bool Included { get; init; }
    }
}
