using AppointmentSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class GetAppointmentByIdConsumer(
        IAppointmentQueryService appointmentQueryService,
        IMapper mapper)
        : RequestConsumer<GetAppointmentByIdEvent, GetAppointmentContract>
    {
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAppointmentContract> Handle(ConsumeContext<GetAppointmentByIdEvent> context)
        {
            var appointmentId = context.Message.Id;

            var appointmentDTOs = await _appointmentQueryService.GetByIdAsync(appointmentId, ignoreRequestingValue: true);
            var appointmentContract = _mapper.Map<GetAppointmentContract>(appointmentDTOs);

            return appointmentContract;
        }
    }
}
