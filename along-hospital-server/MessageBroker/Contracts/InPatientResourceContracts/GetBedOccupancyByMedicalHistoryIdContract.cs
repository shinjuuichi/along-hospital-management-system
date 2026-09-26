using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.InPatientResourceContracts
{
    public record GetBedOccupancyByMedicalHistoryIdContract : BaseContract
    {
        public int Id { get; init; }
        public DateTime FromDateTime { get; init; }
        public DateTime? ToDateTime { get; init; }
        public double DurationInDays { get; init; }
        public double UnitPrice { get; init; }
        public double TotalAmount { get; init; }
        public string? OccupancyStatus { get; init; }
        public string? TransferNote { get; init; }
        public string? LatestTransferNote { get; init; }
        public int MedicalHistoryId { get; init; }
        public int BedId { get; init; }
        public GetBedContract? Bed { get; init; }
    }

    public record GetListBedOccupancyDataContract : BaseContract
    {
        public List<GetBedOccupancyByMedicalHistoryIdContract> Data { get; init; } = [];
    }
}
