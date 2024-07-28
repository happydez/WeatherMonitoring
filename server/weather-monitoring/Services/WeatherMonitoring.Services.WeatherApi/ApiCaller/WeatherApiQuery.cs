using System.Text;

namespace WeatherMonitoring.Services.WeatherApi;

public class WeatherApiQuery
{
    private string _city;
    private string _region;
    private string _country;

    private WeatherApiQuery(string city) { _city = city; }

    public static WeatherApiQueryBuilder CreateBuilder(string city)
    {
        return new WeatherApiQueryBuilder(new WeatherApiQuery(city));
    }

    public override string ToString()
    {
        var sb = new StringBuilder($"{_city}");

        if (_region is not null) sb.Append($",{_region}");

        if (_country is not null) sb.Append($",{_country}");

        return sb.ToString();
    }

    public class WeatherApiQueryBuilder
    {
        private readonly WeatherApiQuery _weatherApiQuery;

        public WeatherApiQueryBuilder(WeatherApiQuery weatherApiQuery)
        {
            _weatherApiQuery = weatherApiQuery;
        }

        public WeatherApiQueryBuilder WithRegion(string region)
        {
            _weatherApiQuery._region = region;
            return this;
        }

        public WeatherApiQueryBuilder WithCountry(string country)
        {
            _weatherApiQuery._country = country;
            return this;
        }

        public WeatherApiQuery Build()
        {
            return _weatherApiQuery;
        }
    }
}
