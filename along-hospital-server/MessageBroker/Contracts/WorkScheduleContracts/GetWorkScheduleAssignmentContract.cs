using MessageBroker.Abstractions;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.TeleSessionContracts;

namespace MessageBroker.Contracts.WorkScheduleContracts
{
    public record GetWorkScheduleAssignmentContract : BaseContract
    {
        public int Id { get; init; }

        public string? LocationType { get; init; }

        public int WorkScheduleId { get; init; }

        public int StaffId { get; init; }

        public int LocationId { get; init; }

        public GetStaffDataByUserIdContract? Staff { get; init; }

        public GetRoomContract? Room { get; init; }

        public GetTeleRoomContract? TeleRoom { get; init; }
    }
}