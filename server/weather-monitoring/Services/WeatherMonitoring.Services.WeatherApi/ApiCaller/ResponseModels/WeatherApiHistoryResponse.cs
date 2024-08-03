using Newtonsoft.Json;

namespace WeatherMonitoring.Services.WeatherApi;

public class WeatherApiHistoryResponse
{
    public Location Location { get; set; }
    public Forecastday Forecastday { get; set; }
    public WeatherCondition Condition { get; set; }
}

public class Forecastday
{
    [JsonProperty("temp_c")]
    public double TemperatureCelsius { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    [JsonProperty("pressure_in")]
    public double PressureIn { get; set; }

    [JsonProperty("wind_kph")]
    public double WindSpeedKph { get; set; }

    [JsonProperty("time_epoch")]
    public long TimeEpoch { get; set; }

    [JsonProperty("time")]
    public DateTime Time { get; set; }
}
