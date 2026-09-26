using MessageBroker.Contracts.StatisticsContracts;

namespace SharedLibrary.Utils
{
    public static class StatisticsContractBuilder
    {
        public static StatisticsChartContract CreateSingleSeriesChart(
            DateOnly fromDate,
            DateOnly toDate,
            string label,
            IReadOnlyDictionary<DateOnly, double> values)
        {
            return new StatisticsChartContract
            {
                Labels = DashboardStatisticsUtil.BuildDateLabels(fromDate, toDate),
                Datasets =
                [
                    new StatisticsChartDatasetContract
                    {
                        Label = label,
                        Data = DashboardStatisticsUtil.BuildDoubleSeries(fromDate, toDate, values)
                    }
                ]
            };
        }

        public static StatisticsChartContract CreateSingleSeriesChart(
            DateOnly fromDate,
            DateOnly toDate,
            string label,
            IReadOnlyDictionary<DateOnly, int> values)
        {
            return new StatisticsChartContract
            {
                Labels = DashboardStatisticsUtil.BuildDateLabels(fromDate, toDate),
                Datasets =
                [
                    new StatisticsChartDatasetContract
                    {
                        Label = label,
                        Data = DashboardStatisticsUtil.BuildDoubleSeries(fromDate, toDate, values)
                    }
                ]
            };
        }

        public static StatisticsChartContract CreateMultiSeriesChart(
            DateOnly fromDate,
            DateOnly toDate,
            params (string Label, IReadOnlyDictionary<DateOnly, int> Values)[] series)
        {
            return new StatisticsChartContract
            {
                Labels = DashboardStatisticsUtil.BuildDateLabels(fromDate, toDate),
                Datasets = series
                    .Select(item => new StatisticsChartDatasetContract
                    {
                        Label = item.Label,
                        Data = DashboardStatisticsUtil.BuildDoubleSeries(fromDate, toDate, item.Values)
                    })
                    .ToList()
            };
        }

        public static StatisticsDistributionContract CreateDistribution<TEnum>(IDictionary<TEnum, int> values)
            where TEnum : struct, Enum
        {
            var orderedValues = Enum.GetValues<TEnum>()
                .Select(value => new
                {
                    Label = value.ToString(),
                    Count = values.TryGetValue(value, out var count) ? count : 0
                })
                .ToList();

            return new StatisticsDistributionContract
            {
                Labels = orderedValues.Select(item => item.Label).ToList(),
                Data = orderedValues.Select(item => item.Count).ToList()
            };
        }

        public static StatisticsDistributionContract CreateDistribution(IEnumerable<(string Label, int Value)> values)
        {
            var orderedValues = values.ToList();

            return new StatisticsDistributionContract
            {
                Labels = orderedValues.Select(item => item.Label).ToList(),
                Data = orderedValues.Select(item => item.Value).ToList()
            };
        }
    }
}
