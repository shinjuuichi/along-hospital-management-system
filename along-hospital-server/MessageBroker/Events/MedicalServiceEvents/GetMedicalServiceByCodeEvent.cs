using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalServiceEvents
{
    public record GetMedicalServiceByCodeEvent : BaseEvent
    {
        public string? Code { get; init; }
    }

    public record GetListMedicalServiceDataByCodesEvent : BaseEvent
    {
        public List<string> Codes { get; init; } = [];
    }
}