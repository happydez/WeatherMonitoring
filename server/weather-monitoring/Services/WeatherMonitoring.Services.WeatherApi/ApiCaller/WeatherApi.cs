using Newtonsoft.Json;

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
