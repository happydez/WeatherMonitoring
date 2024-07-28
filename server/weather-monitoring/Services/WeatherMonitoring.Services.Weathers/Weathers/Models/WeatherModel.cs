namespace WeatherMonitoring.Services.Weathers;

public class WeatherModel
{
    public int LocationId { get; set; }
    public string Condition { get; set; }
    public double TemperatureCelsius { get; set; }
    public int Humidity { get; set; }
    public double PressureIn { get; set; }
    public double WindSpeedKph { get; set; }
    public long LastUpdatedEpoch { get; set; }
    public DateTime LastUpdated { get; set; }
}