using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace WeatherMonitoring.Common;

public static class FileExtensions
{
    public static IEnumerable<TData> ReadCsvFile<TData, TMap>(string path) 
        where TData : class, new()
        where TMap : ClassMap<TData>
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<TMap>();

        var records = csv.GetRecords<TData>().ToList();

        return records;
    }
}
