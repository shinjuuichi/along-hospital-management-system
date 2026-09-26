using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicalHistoryContracts
{
    public record CreateMedicalHistoryFromAppointmentContract : BaseContract
    {
        public int MedicalHistoryId { get; init; }
    }
}
