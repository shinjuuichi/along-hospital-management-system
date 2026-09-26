using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicalHistoryContracts
{
    public record GetMedicalHistoryContract : BaseContract
    {
        public int Id { get; init; }

        public string? MedicalHistoryNumber { get; init; }

        public string? Diagnosis { get; init; }

        public DateOnly? FollowUpAppointmentDate { get; init; }

        public string? MedicalHistoryStatus { get; init; }

        public string? MedicalHistoryType { get; init; }

        public DateTime AdmissionDate { get; init; }

        public DateTime? DischargeDate { get; init; }

        public int PatientId { get; init; }

        public int? DoctorId { get; init; }

        public int SpecialtyId { get; init; }
    }

    public record GetListMedicalHistoryDataContract : BaseContract
    {
        public List<GetMedicalHistoryContract> Data { get; init; } = [];
    }
}
