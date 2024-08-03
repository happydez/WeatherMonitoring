namespace WeatherMonitoring.Services.WeatherApi;

public interface IWeatherApi
{
    Task<WeatherApiResponse> GetWeatherDataAsync(WeatherApiQuery weatherApiQuery);
    Task<IEnumerable<WeatherApiSearchResponse>> SearchLocationAsync(WeatherApiQuery weatherApiQuery);
    Task<IEnumerable<WeatherApiHistoryResponse>> GetWeatherHistoryAsync(WeatherApiQuery weatherApiQuery, DateOnly date);
}
