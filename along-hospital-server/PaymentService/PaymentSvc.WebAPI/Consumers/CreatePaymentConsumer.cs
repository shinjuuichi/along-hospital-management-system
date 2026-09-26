using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Events.PaymentEvents;
using PaymentSvc.BLL.DTOs.CashDTOs;
using PaymentSvc.BLL.DTOs.PayOSDTOs;
using PaymentSvc.BLL.DTOs.SePayDTOs;
using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PaymentSvc.WebAPI.Consumers
{
    public class CreatePaymentConsumer(
        IPayOSService payOSService,
        ICashService cashService,
        ISePayService sePayService,
        IMapper mapper)
        : RequestConsumer<CreatePaymentEvent, CreatePaymentContract>
    {
        private readonly IPayOSService _payOSService = payOSService;
        private readonly ICashService _cashService = cashService;
        private readonly ISePayService _sePayService = sePayService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<CreatePaymentContract> Handle(ConsumeContext<CreatePaymentEvent> context)
        {
            var message = context.Message;
            if (message.PaymentEventItems == null || message.PaymentEventItems.Count == 0)
            {
                throw new InvalidDataException("No payment items provided in the event.");
            }

            string paymentUrl = string.Empty;
            Guid transactionId;

            switch (message.Provider?.ToUpperInvariant())
            {
                case "PAYOS":
                    var payOsRequest = _mapper.Map<CreatePayOSDTO>(message);
                    var payOsResult = await _payOSService.CreatePayOSPaymentAsync(payOsRequest);

                    paymentUrl = payOsResult.PaymentUrl!;
                    transactionId = payOsResult.TransactionId;
                    break;

                case "CASH":
                    var cashRequest = _mapper.Map<CreateCashDTO>(message);
                    var cashResult = await _cashService.CreateCashPaymentAsync(cashRequest);

                    transactionId = cashResult.TransactionId;
                    break;

                case "SEPAY":
                    var sePayRequest = _mapper.Map<CreateSePayDTO>(message);
                    var sePayResult = await _sePayService.CreateSePayPaymentAsync(sePayRequest);

                    paymentUrl = sePayResult.PaymentUrl!;
                    transactionId = sePayResult.TransactionId;
                    break;

                default:
                    throw new InvalidDataException($"Unsupported payment provider: {message.Provider}");
            }

            return new CreatePaymentContract
            {
                TransactionId = transactionId,
                PaymentUrl = paymentUrl
            };
        }
    }
}
