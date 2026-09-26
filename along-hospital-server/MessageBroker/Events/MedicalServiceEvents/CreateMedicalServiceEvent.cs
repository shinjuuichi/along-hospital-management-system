using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalServiceEvents
{
    public record CreateMedicalServiceEvent : BaseEvent
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public double Price { get; init; }
        public int SpecialtyId { get; init; }
    }
}