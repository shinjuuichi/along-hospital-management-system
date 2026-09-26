using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GetTodayWorkingDoctorByRoomIdEvent : BaseEvent
    {
        public int RoomId { get; set; }
    }

    public record GetTodayWorkingDoctorByRoomIdContract : BaseContract
    {
        public int? DoctorId { get; set; }
    }
}