using MessageBroker.Abstractions;

namespace MessageBroker.Events.TeleHealthEvents.TeleRoomEvents
{
    public record GetTeleRoomByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }
}