using Newtonsoft.Json;

namespace WeatherMonitoring.Entities.WeatherAPIModels
{
    public class APIWeather
    {
        [JsonProperty("condition")]
        public APIWeatherCondition? Condition { get; set; }

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
}
