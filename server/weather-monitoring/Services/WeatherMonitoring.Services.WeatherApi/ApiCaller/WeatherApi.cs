using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherMonitoring.Services.WeatherApi;

public class WeatherApi : IWeatherApi
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiSettings _weatherApiSettings;

    public WeatherApi(HttpClient httpClient, WeatherApiSettings weatherApiSettings)
    {
        _httpClient = httpClient;
        _weatherApiSettings = weatherApiSettings;
    }

    public async Task<IEnumerable<WeatherApiSearchResponse>> SearchLocationAsync(WeatherApiQuery weatherApiQuery)
    {
        var response = await _httpClient.GetAsync($"search.json?key={_weatherApiSettings.ApiKey}&q={weatherApiQuery}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var weatherApiSearchResponse = JsonConvert.DeserializeObject<IEnumerable<WeatherApiSearchResponse>>(content);

        return weatherApiSearchResponse;
    }

    public async Task<IEnumerable<WeatherApiHistoryResponse>> GetWeatherHistoryAsync(WeatherApiQuery weatherApiQuery, DateOnly date)
    {
        var response = await _httpClient.GetAsync($"history.json?" +
            $"key={_weatherApiSettings.ApiKey}" +
            $"&q={weatherApiQuery}" +
            $"&dt={date:yyyy-MM-dd}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var serializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        var jsonObject = JsonConvert.DeserializeObject<JObject>(content, serializerSettings);
        var location = jsonObject["location"];
        var forecastDays = jsonObject["forecast"]["forecastday"];

        var weatherHistoryList = new List<WeatherApiHistoryResponse>();

        var weatherLocation = new Location
        {
            Name = (string)location["name"],
            Region = (string)location["region"],
            Country = (string)location["country"]
        };

        foreach (var day in forecastDays)
        {
            var hourlyData = day["hour"];

            foreach (var hour in hourlyData)
            {
                var weatherForecastday = new Forecastday
                {
                    TemperatureCelsius = (double)hour["temp_c"],
                    Humidity = (int)hour["humidity"],
                    PressureIn = (double)hour["pressure_in"],
                    WindSpeedKph = (double)hour["wind_kph"],
                    TimeEpoch = (long)hour["time_epoch"],
                    Time = DateTime.SpecifyKind((DateTime)hour["time"], DateTimeKind.Utc)
                };

                var weatherCondition = new WeatherCondition
                {
                    Code = (int)hour["condition"]["code"],
                    Text = (string)hour["condition"]["text"],
                };

                weatherHistoryList.Add(new WeatherApiHistoryResponse
                { 
                    Location = weatherLocation,
                    Forecastday = weatherForecastday,
                    Condition = weatherCondition
                });
            }
        }

        return weatherHistoryList;
    }

    public async Task<WeatherApiResponse> GetWeatherDataAsync(WeatherApiQuery weatherApiQuery)
    {
        var response = await _httpClient.GetAsync($"current.json?key={_weatherApiSettings.ApiKey}&q={weatherApiQuery}");

        response.EnsureSuccessStatusCode();

        var serializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        var content = await response.Content.ReadAsStringAsync();
        var weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(content, serializerSettings);

        return weatherApiResponse;
    }
}
