using AutoMapper;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class GetInventoryBySKUCodeConsumer(
        IInventoryService _inventoryService,
        IMapper _mapper)
        : RequestConsumer<GetInventoryBySKUCodeEvent, GetInventoryBySKUCodeContract>
    {
        protected override async Task<GetInventoryBySKUCodeContract> Handle(
            ConsumeContext<GetInventoryBySKUCodeEvent> context)
        {
            var skuCode = context.Message.SKUCode ?? throw new ArgumentNullException(nameof(context.Message.SKUCode), "SKUCode cannot be null");
            var inventoryDTO = await _inventoryService.GetInventoryBySKUCodeAsync(skuCode);
            return _mapper.Map<GetInventoryBySKUCodeContract>(inventoryDTO);
        }
    }
}