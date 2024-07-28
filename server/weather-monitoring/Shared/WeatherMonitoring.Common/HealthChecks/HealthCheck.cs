namespace WeatherMonitoring.Common;

public class HealthCheck
{
    public string OverallStatus { get; set; }
    public IEnumerable<HealthCheckItem> HealthChecks { get; set; }
}

public class HealthCheckItem
{
    public string Status { get; set; }
    public string Component { get; set; }
    public string Description { get; set; }
}