using AutoMapper;
using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class DecreaseInventoryQuantityConsumer(IInventoryService _inventoryService,
        IMapper _mapper)
       : RequestConsumer<DecreaseInventoryQuantityEvent, DecreaseMedicineQuantityContract>
    {
        protected override async Task<DecreaseMedicineQuantityContract> Handle(ConsumeContext<DecreaseInventoryQuantityEvent> context)
        {
            var decreaseItems = context.Message.DecreaseInventoryQuantityEventItems;

            foreach (var item in decreaseItems)
            {
                if (string.IsNullOrWhiteSpace(item.SKUCode))
                {
                    throw new InvalidDataException("SKUCode cannot be null or empty");
                }

                var inventoryDto = await _inventoryService.GetInventoryBySKUCodeAsync(item.SKUCode);
                var newQuantity = inventoryDto.Quantity - item.Quantity;
                var updateDto = _mapper.Map<UpdateInventoryDTO>(inventoryDto);
                updateDto.Quantity = newQuantity;
                await _inventoryService.UpdateBySKUCodeAsync(item.SKUCode, updateDto);
            }

            return new DecreaseMedicineQuantityContract { IsSuccess = true };
        }
    }
}