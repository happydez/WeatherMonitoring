using Serilog;
using Serilog.Events;
using WeatherMonitoring.Services.Settings;

using WMLogLevel = WeatherMonitoring.Services.Settings.LogLevel;
using WMLogRollingInterval = WeatherMonitoring.Services.Settings.LogRollingInterval;

namespace WeatherMonitoring.Api.Configuration;

/// <summary>
/// Logger Configuration
/// </summary>
public static class LoggerConfiguration
{
    /// <summary>
    /// Add logger
    /// </summary>
    public static void AddAppLogger(this WebApplicationBuilder builder, MainSettings mainSettings, LogSettings logSettings)
    {
        var loggerConfiguration = new Serilog.LoggerConfiguration();

        // Base configuration
        loggerConfiguration
            .Enrich.WithCorrelationIdHeader()
            .Enrich.FromLogContext();

        // Log level
        if (!Enum.TryParse(logSettings.Level, out WMLogLevel level))
        {
            level = WMLogLevel.Information;
        }

        var serilogLevel = level switch
        {
            WMLogLevel.Verbose => LogEventLevel.Verbose,
            WMLogLevel.Debug => LogEventLevel.Debug,
            WMLogLevel.Information => LogEventLevel.Information,
            WMLogLevel.Warning => LogEventLevel.Warning,
            WMLogLevel.Error => LogEventLevel.Error,
            WMLogLevel.Fatal => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };

        loggerConfiguration
            .MinimumLevel.Is(serilogLevel)
            .MinimumLevel.Override("Microsoft", serilogLevel)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", serilogLevel)
            .MinimumLevel.Override("System", serilogLevel)
            ;

        // Writers
        var logItemTemplate =
            "[{Timestamp:HH:mm:ss:fff} {Level:u3} ({CorrelationId})] {Message:lj}{NewLine}{Exception}";

        // Writing to Console configuration 
        if (logSettings.WriteToConsole)
        {
            loggerConfiguration.WriteTo.Console(
                serilogLevel,
                logItemTemplate
            );
        }

        // Writing to File configuration 
        if (logSettings.WriteToFile)
        {
            if (!Enum.TryParse(logSettings.FileRollingInterval, out WMLogRollingInterval interval))
            {
                interval = WMLogRollingInterval.Day;
            }

            var serilogInterval = interval switch
            {
                WMLogRollingInterval.Infinite => RollingInterval.Infinite,
                WMLogRollingInterval.Year => RollingInterval.Year,
                WMLogRollingInterval.Month => RollingInterval.Month,
                WMLogRollingInterval.Day => RollingInterval.Day,
                WMLogRollingInterval.Hour => RollingInterval.Hour,
                WMLogRollingInterval.Minute => RollingInterval.Minute,
                _ => RollingInterval.Day
            };

            if (!int.TryParse(logSettings.FileRollingSize, out var size))
            {
                size = 5242880;
            }

            var fileName = $"_.log";

            loggerConfiguration.WriteTo.File($"logs/{fileName}",
                serilogLevel,
                logItemTemplate,
                rollingInterval: serilogInterval,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: size
            );
        }

        // Make logger
        var logger = loggerConfiguration.CreateLogger();

        // Apply logger to application
        builder.Host.UseSerilog(logger, true);
    }
}
