namespace SharedLibrary.Utils
{
    public static class DashboardStatisticsUtil
    {
        public static DateTime ToUtcStart(DateOnly date)
            => new(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToUtcEndExclusive(DateOnly date)
            => ToUtcStart(date).AddDays(1);

        public static List<DateOnly> GetDatesInRange(DateOnly fromDate, DateOnly toDate)
        {
            var dates = new List<DateOnly>();
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }

        public static List<string> BuildDateLabels(DateOnly fromDate, DateOnly toDate, string format = "yyyy-MM-dd")
            => GetDatesInRange(fromDate, toDate)
                .Select(date => date.ToString(format))
                .ToList();

        public static List<double> BuildDoubleSeries(
            DateOnly fromDate,
            DateOnly toDate,
            IReadOnlyDictionary<DateOnly, double> values)
        {
            return GetDatesInRange(fromDate, toDate)
                .Select(date => values.TryGetValue(date, out var value) ? value : 0d)
                .ToList();
        }

        public static List<double> BuildDoubleSeries(
            DateOnly fromDate,
            DateOnly toDate,
            IReadOnlyDictionary<DateOnly, int> values)
        {
            return GetDatesInRange(fromDate, toDate)
                .Select(date => values.TryGetValue(date, out var value) ? Convert.ToDouble(value) : 0d)
                .ToList();
        }
    }
}
