using AutoMapper;
using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class AddQuantityFromImportConsumer(
        IInventoryService _inventoryService,
        IMapper _mapper)
        : RequestConsumer<AddQuantityFromImportEvent, AddQuantityFromImportContract>
    {
        protected override async Task<AddQuantityFromImportContract> Handle(
            ConsumeContext<AddQuantityFromImportEvent> context)
        {
            if (string.IsNullOrWhiteSpace(context.Message.SKUCode))
            {
                throw new InvalidDataException("SKUCode cannot be null or empty.");
            }

            var inventoryDto = await _inventoryService.GetInventoryBySKUCodeAsync(context.Message.SKUCode);

            if (inventoryDto.MaxQuantity.HasValue && context.Message.Quantity > inventoryDto.MaxQuantity.Value)
            {
                throw new InvalidDataException(
                    $"Quantity ({context.Message.Quantity}) exceeds MaxQuantity ({inventoryDto.MaxQuantity.Value}) " +
                    $"for SKU {context.Message.SKUCode}");
            }

            var updateDto = _mapper.Map<UpdateInventoryDTO>(context.Message);
            updateDto.MinQuantity = inventoryDto.MinQuantity;
            updateDto.MaxQuantity = inventoryDto.MaxQuantity;

            await _inventoryService.UpdateBySKUCodeAsync(context.Message.SKUCode, updateDto);
            return new() { IsSuccess = true };
        }
    }
}