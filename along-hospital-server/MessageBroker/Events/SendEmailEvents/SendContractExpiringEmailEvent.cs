using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendContractExpiringEmailEvent : BaseEvent
    {
        public string? Email { get; set; }

        public string? StaffName { get; set; }

        public DateOnly ContractEndDate { get; set; }

        public int DaysRemaining { get; set; }
    }
}
