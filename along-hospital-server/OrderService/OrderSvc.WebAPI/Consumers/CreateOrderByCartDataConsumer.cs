using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Events.OrderEvents;
using OrderSvc.BLL.DTOs;
using OrderSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace OrderSvc.WebAPI.Consumers
{
    public class CreateOrderByCartDataConsumer(
        IMapper mapper,
        IOrderService orderService)
            : RequestConsumer<CreateOrderByCartDataEvent, CreateOrderByCartDataContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IOrderService _orderService = orderService;

        protected override async Task<CreateOrderByCartDataContract> Handle(ConsumeContext<CreateOrderByCartDataEvent> context)
        {
            var createOrderDTO = _mapper.Map<CreateOrderDTO>(context.Message);
            var createOrderByCartDataContract = await _orderService.CreateAsync(createOrderDTO);
            return new() { PaymentUrl = createOrderByCartDataContract.PaymentUrl };
        }
    }
}
