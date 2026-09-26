using MassTransit;
using MessageBroker.Events.OrderEvents;
using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PaymentSvc.WebAPI.Consumers
{
    public class UpdateOrderStatusConsumer(ICashService cashService) : EventConsumer<UpdateOrderStatusEvent>
    {
        private readonly ICashService _cashService = cashService;

        protected override async Task Handle(ConsumeContext<UpdateOrderStatusEvent> context)
        {
            await _cashService.UpdatePaymentStatus(context.Message.TransactionId);
        }
    }
}