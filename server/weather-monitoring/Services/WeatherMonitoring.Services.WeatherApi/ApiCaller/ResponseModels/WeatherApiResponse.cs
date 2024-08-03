using Newtonsoft.Json;

namespace WeatherMonitoring.Services.WeatherApi;

public class WeatherApiResponse
{
    public Location Location { get; set; }
    public CurrentWeather Current { get; set; }
}

public class Location
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("lat")]
    public double Lat { get; set; }

    [JsonProperty("lon")]
    public double Lon { get; set; }

    [JsonProperty("tz_id")]
    public string TzId { get; set; }

    [JsonProperty("localtime_epoch")]
    public long LocaltimeEpoch { get; set; }

    [JsonProperty("localtime")]
    public DateTime Localtime { get; set; }
}

public class CurrentWeather
{
    /// <summary>
    /// Weather Condition code
    /// </summary>
    [JsonProperty("condition")]
    public WeatherCondition Condition { get; set; }

    [JsonProperty("temp_c")]
    public double TemperatureCelsius { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    [JsonProperty("pressure_in")]
    public double PressureIn { get; set; }

    [JsonProperty("wind_kph")]
    public double WindSpeedKph { get; set; }

    [JsonProperty("last_updated_epoch")]
    public long LastUpdatedEpoch { get; set; }

    [JsonProperty("last_updated")]
    public DateTime LastUpdated { get; set; }
}

public class WeatherCondition
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("code")]
    public int Code { get; set; }
}
