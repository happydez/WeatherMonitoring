namespace WeatherMonitoring.Context.Entities;

public class Weather : BaseEntity
{
    public int LocationId { get; set; }
    public virtual Location Location { get; set; } = null!;
    public int ConditionCode { get; set; }
    public string ConditionText { get; set; }
    public double TemperatureCelsius { get; set; }
    public int Humidity { get; set; }
    public double PressureIn { get; set; }
    public double WindSpeedKph { get; set; }
    public long LastUpdatedEpoch { get; set; }
    public DateTime LastUpdated { get; set; }
}