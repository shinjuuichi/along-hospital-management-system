using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.InPatientResourceContracts
{
    public record GetBedContract : BaseContract
    {
        public int Id { get; init; }
        public string? Code { get; init; }
        public string? Status { get; init; }
        public int BedCategoryId { get; init; }
        public string? BedCategoryCode { get; init; }
        public string? BedCategoryName { get; init; }
        public GetRoomContract? Room { get; init; }
    }
}