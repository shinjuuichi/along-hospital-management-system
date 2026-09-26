using AutoMapper;
using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class AddQuantityFromCancelOrderConsumer(
        IInventoryService _inventoryService,
        IMapper _mapper)
        : RequestConsumer<AddQuantityFromCancelOrderEvent, AddQuantityFromCancelOrderContract>
    {
        protected override async Task<AddQuantityFromCancelOrderContract> Handle(
            ConsumeContext<AddQuantityFromCancelOrderEvent> context)
        {
            var items = context.Message.Items;

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.SKUCode))
                {
                    throw new InvalidDataException("SKUCode cannot be null or empty");
                }

                if (item.Quantity <= 0)
                {
                    throw new InvalidDataException("Quantity must be greater than zero");
                }

                var inventoryDto = await _inventoryService.GetInventoryBySKUCodeAsync(item.SKUCode);
                var newQuantity = inventoryDto.Quantity + item.Quantity;
                var updateDto = _mapper.Map<UpdateInventoryDTO>(inventoryDto);
                updateDto.Quantity = newQuantity;
                await _inventoryService.UpdateAsync(inventoryDto.Id, updateDto);
            }

            return new AddQuantityFromCancelOrderContract { IsSuccess = true };
        }
    }
}
