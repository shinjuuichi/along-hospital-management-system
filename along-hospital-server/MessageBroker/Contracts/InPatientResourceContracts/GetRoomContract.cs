using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.InPatientResourceContracts
{
    public record GetRoomContract : BaseContract
    {
        public int Id { get; init; }
        public string? Code { get; init; }
        public string? Status { get; init; }

        public int BuildingId { get; init; }
        public string? BuildingName { get; init; }

        public int FloorId { get; init; }
        public int FloorNumber { get; init; }

        public int RoomCategoryId { get; init; }
        public string? RoomCategoryName { get; init; }
        public List<string> Roles { get; init; } = [];

        public int SpecialtyId { get; init; }
        public string? SpecialtyName { get; init; }

        public int BedCount { get; init; }
        public int AvailableBedCount { get; init; }
    }

    public record GetListRoomDataContract : BaseContract
    {
        public List<GetRoomContract> Data { get; init; } = [];
    }
}