using AutoMapper;
using MassTransit;
using MessageBroker.Events.PaymentEvents;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PayrollSvc.WebAPI.Consumers
{
    public class PayrollPaymentStatusChangedConsumer(
        IPayrollService payrollService,
        IMapper mapper)
        : EventConsumer<PaymentStatusChangedEvent>
    {
        private readonly IPayrollService _payrollService = payrollService;
        private readonly IMapper _mapper = mapper;

        protected override async Task Handle(ConsumeContext<PaymentStatusChangedEvent> context)
        {
            var paymentStatusChangedDTO = _mapper.Map<PaymentStatusChangedDTO>(context.Message);
            await _payrollService.HandlePaymentStatusChangedAsync(paymentStatusChangedDTO);
        }
    }
}
