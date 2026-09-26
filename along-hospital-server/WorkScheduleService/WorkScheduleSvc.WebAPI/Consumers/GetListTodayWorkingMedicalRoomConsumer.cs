using AutoMapper;
using MassTransit;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces.Querys;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class GetListTodayWorkingMedicalRoomConsumer(
        IWorkScheduleQueryService workScheduleQueryService,
        IMapper mapper) : RequestConsumer<GetListTodayWorkingMedicalRoomEvent, GetListTodayWorkingMedicalRoomContract>
    {
        private readonly IWorkScheduleQueryService _workScheduleQueryService = workScheduleQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListTodayWorkingMedicalRoomContract> Handle(ConsumeContext<GetListTodayWorkingMedicalRoomEvent> context)
        {
            var roomDoctorInfoDTOs = await _workScheduleQueryService.GetListTodayWorkingByQueueManagementRoleAsync();

            var data = _mapper.Map<List<GetListTodayWorkingMedicalRoomContract.RoomDoctorInfo>>(roomDoctorInfoDTOs);
            return new GetListTodayWorkingMedicalRoomContract { Data = data };
        }
    }
}