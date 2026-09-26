using CartSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.CartEvents;
using SharedLibrary.Base.MessageBuses;

namespace CartSvc.WebAPI.Consumers
{
    public class CreateCartConsumer(ICartService cartService) : RequestConsumer<CreateCartEvent, CreateCartContract>
    {
        private readonly ICartService _cartService = cartService;

        protected override async Task<CreateCartContract> Handle(ConsumeContext<CreateCartEvent> context)
        {
            await _cartService.CreateAsync(context.Message.PatientId);
            return new CreateCartContract();
        }
    }
}
