namespace WeatherMonitoring.Context.Seeder;

using Newtonsoft.Json;
using WeatherMonitoring.Context.Entities;

#pragma warning disable IDE0090

public class DemoHelper
{
    public IEnumerable<Location> GetLocations
    {
        get
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                @"Seeds/Demo/WeatherDemoData",
                "locations.json");

            var json = File.ReadAllText(path);
            var locations = JsonConvert.DeserializeObject<IEnumerable<Location>>(json);

            return locations;
        }
    }

}