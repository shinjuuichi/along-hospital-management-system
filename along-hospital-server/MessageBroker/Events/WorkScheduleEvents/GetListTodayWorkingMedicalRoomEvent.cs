using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GetListTodayWorkingMedicalRoomEvent : BaseEvent;

    public record GetListTodayWorkingMedicalRoomContract : BaseContract
    {
        public List<RoomDoctorInfo> Data { get; init; } = [];

        public record RoomDoctorInfo
        {
            public int RoomId { get; init; }

            public int? DoctorId { get; init; }
        }
    }
}