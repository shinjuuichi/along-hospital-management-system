using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Events.PaymentEvents;
using PaymentSvc.BLL.DTOs.SePayDTOs;
using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PaymentSvc.WebAPI.Consumers
{
    public class CreatePaymentForPayrollConsumer(
        ISePayService sePayService,
        IMapper mapper)
        : RequestConsumer<CreatePaymentEventForPayroll, CreatePaymentForPayrollContract>
    {
        private readonly ISePayService _sePayService = sePayService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<CreatePaymentForPayrollContract> Handle(ConsumeContext<CreatePaymentEventForPayroll> context)
        {
            var message = context.Message;

            var sePayRequest = _mapper.Map<CreateSePayDTO>(message);
            var sePayResult = await _sePayService.CreateSePayForPayrollAsync(sePayRequest);

            return _mapper.Map<CreatePaymentForPayrollContract>(sePayResult);
        }
    }
}
