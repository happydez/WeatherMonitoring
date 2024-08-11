using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace WeatherMonitoring.Common.Helpers
{
    public static class CsvHelper
    {
        public static IEnumerable<TData> ReadCsvFile<TData, TMap>(string filePath)
            where TData : class, new()
            where TMap : ClassMap<TData>
        {
            var config = new CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<TMap>();

            var records = csv.GetRecords<TData>().ToList();

            return records;
        }
    }
}
