namespace MessageBroker.Contracts.StatisticsContracts
{
    public record StatisticsChartContract
    {
        public List<string> Labels { get; init; } = [];
        public List<StatisticsChartDatasetContract> Datasets { get; init; } = [];
    }

    public record StatisticsChartDatasetContract
    {
        public string Label { get; init; } = string.Empty;
        public List<double> Data { get; init; } = [];
    }

    public record StatisticsDistributionContract
    {
        public List<string> Labels { get; init; } = [];
        public List<int> Data { get; init; } = [];
    }
}
