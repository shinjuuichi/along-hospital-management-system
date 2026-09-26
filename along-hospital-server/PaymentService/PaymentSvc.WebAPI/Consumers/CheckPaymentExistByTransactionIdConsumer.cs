using MassTransit;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Events.PaymentEvents;
using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PaymentSvc.WebAPI.Consumers
{
    public class CheckPaymentExistByTransactionIdConsumer(
        IPayOSService _payOSService)
        : RequestConsumer<CheckPaymentExistByTransactionIdEvent, CheckPaymentExistByTransactionIdContract>
    {
        private readonly IPayOSService _payOSService = _payOSService;

        protected override async Task<CheckPaymentExistByTransactionIdContract> Handle(ConsumeContext<CheckPaymentExistByTransactionIdEvent> context)
        {
            var paymentUrl = await _payOSService.GetPaymentUrlByTransactionIdAsync(context.Message.TransactionId);

            return new CheckPaymentExistByTransactionIdContract
            {
                PaymentUrl = paymentUrl,
            };
        }
    }
}