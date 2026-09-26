using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicalHistoryContracts
{
    public record GetMedicalHistoryOccupancySummaryContract : BaseContract
    {
        public int Id { get; init; }

        public string? MedicalHistoryNumber { get; init; }

        public int PatientId { get; init; }

        public string? PatientName { get; init; }

        public int? DoctorId { get; init; }

        public string? DoctorName { get; init; }

        public string? MedicalHistoryStatus { get; init; }

        public DateTime AdmissionDate { get; init; }

        public DateTime? DischargeDate { get; init; }
    }

    public record GetListMedicalHistoryOccupancySummaryDataContract : BaseContract
    {
        public List<GetMedicalHistoryOccupancySummaryContract> Data { get; init; } = [];
    }
}
