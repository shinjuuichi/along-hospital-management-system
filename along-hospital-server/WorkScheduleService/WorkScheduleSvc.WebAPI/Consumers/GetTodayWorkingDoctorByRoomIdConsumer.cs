using MassTransit;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces.Querys;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class GetTodayWorkingDoctorByRoomIdConsumer(
        IWorkScheduleQueryService workScheduleQueryService)
        : RequestConsumer<GetTodayWorkingDoctorByRoomIdEvent, GetTodayWorkingDoctorByRoomIdContract>
    {
        private readonly IWorkScheduleQueryService _workScheduleQueryService = workScheduleQueryService;

        protected override async Task<GetTodayWorkingDoctorByRoomIdContract> Handle(ConsumeContext<GetTodayWorkingDoctorByRoomIdEvent> context)
        {
            var request = context.Message;
            var doctorId = await _workScheduleQueryService.GetTodayWorkingDoctorIdByRoomIdAsync(request.RoomId);

            return new GetTodayWorkingDoctorByRoomIdContract
            {
                DoctorId = doctorId
            };
        }
    }
}