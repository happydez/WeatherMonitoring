using Newtonsoft.Json;

namespace WeatherMonitoring.Entities.WeatherAPIModels
{
    public class APIWeatherCondition
    {
        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
