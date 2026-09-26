using MassTransit;
using MessageBroker.Events.PaymentEvents;
using OrderSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace OrderSvc.WebAPI.Consumers
{
    public class OrderPaymentStatusChangedConsumer(IOrderService orderService)
        : EventConsumer<PaymentStatusChangedEvent>
    {
        private readonly IOrderService _orderService = orderService;

        protected override async Task Handle(ConsumeContext<PaymentStatusChangedEvent> context)
        {
            var transactionId = context.Message.TransactionId;
            var paymentStatus = context.Message.PaymentStatus;
            await _orderService.HandlePaymentStatusChangedAsync(transactionId, paymentStatus);
        }
    }
}
