using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.TeleSessionContracts
{
    public record GetTeleRoomContract : BaseContract
    {
        public int Id { get; init; }

        public string? RoomCode { get; init; }

        public string? RoomDisplayName { get; init; }

        public int SpecialtyId { get; init; }
    }
}
