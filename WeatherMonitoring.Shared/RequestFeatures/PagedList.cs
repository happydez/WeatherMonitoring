namespace WeatherMonitoring.Shared.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int offset, int limit)
        {
            MetaData = new()
            {
                TotalCount = count,
                Offset = offset,
                Limit = limit
            };

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int offset, int limit)
        {
            var count = source.Count();
            var items = source.Skip(offset).Take(limit).ToList();

            return new PagedList<T>(items, count, offset, limit);
        }
    }
}
