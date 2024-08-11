using Newtonsoft.Json;

namespace WeatherMonitoring.Entities.WeatherAPIModels
{
    public class APIResponse
    {
        public APILocation? Location { get; set; }

        [JsonProperty("current")]
        public APIWeather? Weather { get; set; }
    }
}
