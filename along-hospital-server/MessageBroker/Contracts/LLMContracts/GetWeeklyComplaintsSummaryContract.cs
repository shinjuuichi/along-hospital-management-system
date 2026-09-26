using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.LLMContracts
{
    public record GetWeeklyComplaintsSummaryContract : BaseContract
    {
        public string? Summary { get; init; }
    }
}