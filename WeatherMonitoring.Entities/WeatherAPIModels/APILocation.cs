using Newtonsoft.Json;

namespace WeatherMonitoring.Entities.WeatherAPIModels
{
    public class APILocation
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("region")]
        public string? Region { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("tz_id")]
        public string? TzId { get; set; }
    }
}
