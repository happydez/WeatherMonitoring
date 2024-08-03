namespace WeatherMonitoring.Context.Entities;

public class Location : BaseEntity
{
    public string Name { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string TzId { get; set; }
    public bool Active { get; set; }
    public bool Included { get; set; }

    public virtual ICollection<Weather> Weathers { get; set; } = [];
}