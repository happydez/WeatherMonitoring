namespace WeatherMonitoring.Common.Extensions;

public static class DateExtension
{

    public static long ToEpoch(this DateTime date, string timeZoneId)
    {
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        DateTime dateTimeWithKind = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        DateTimeOffset dateTimeOffset = TimeZoneInfo.ConvertTimeToUtc(dateTimeWithKind, timeZoneInfo);

        return dateTimeOffset.ToUnixTimeSeconds();
    }
}
