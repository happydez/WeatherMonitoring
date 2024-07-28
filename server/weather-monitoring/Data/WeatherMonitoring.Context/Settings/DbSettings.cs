namespace WeatherMonitoring.Context;

public class DbSettings
{
    public DbType Type { get; private set; }
    public string ConnectionString { get; set; }
    public DbInitSettings Init { get; set; }
}

public class DbInitSettings
{
    public bool AddDemoData { get; private set; }

    public bool AddAdministrator { get; private set; }
    public UserCredentials Administrator { get; private set; }
}

public class UserCredentials
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
}