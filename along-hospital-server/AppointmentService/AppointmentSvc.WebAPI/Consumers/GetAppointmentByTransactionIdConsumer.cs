using AppointmentSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class GetAppointmentByTransactionIdConsumer(
        IAppointmentQueryService appointmentQueryService,
        IMapper mapper)
        : RequestConsumer<GetAppointmentByTransactionIdEvent, GetAppointmentContract>
    {
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAppointmentContract> Handle(ConsumeContext<GetAppointmentByTransactionIdEvent> context)
        {
            var appointmentDTO = await _appointmentQueryService.GetByTransactionIdAsync(context.Message.TransactionId);
            return _mapper.Map<GetAppointmentContract>(appointmentDTO);
        }
    }
}
