using AppointmentSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class GetListAppointmentDataByIdsConsumer(
        IAppointmentQueryService appointmentQueryService,
        IMapper mapper)
        : RequestConsumer<GetListAppointmentDataByIdsEvent, GetListAppointmentDataContract>
    {
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListAppointmentDataContract> Handle(ConsumeContext<GetListAppointmentDataByIdsEvent> context)
        {
            var appointmentIds = context.Message.Ids;

            var appointmentDTOs = await _appointmentQueryService.GetAllByIdsAsync(appointmentIds, ignoreRequestingValue: true);
            var appointmentContracts = _mapper.Map<List<GetAppointmentContract>>(appointmentDTOs);

            return new() { Data = appointmentContracts };
        }
    }
}