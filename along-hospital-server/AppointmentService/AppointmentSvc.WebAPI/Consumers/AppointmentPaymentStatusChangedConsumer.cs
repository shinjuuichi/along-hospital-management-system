using AppointmentSvc.BLL.DTOs;
using AppointmentSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Events.PaymentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class AppointmentPaymentStatusChangedConsumer(
        IAppointmentCommandService appointmentCommandService,
        IMapper mapper)
            : EventConsumer<PaymentStatusChangedEvent>
    {
        private readonly IAppointmentCommandService _appointmentCommandService = appointmentCommandService;
        private readonly IMapper _mapper = mapper;

        protected override async Task Handle(ConsumeContext<PaymentStatusChangedEvent> context)
        {
            var paymentStatusChangedDTO = _mapper.Map<PaymentStatusChangedDTO>(context.Message);
            await _appointmentCommandService.HandlePaymentStatusChangedAsync(paymentStatusChangedDTO);
        }
    }
}
