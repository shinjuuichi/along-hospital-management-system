using AutoMapper;
using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class UpdateInventoryConsumer(IInventoryService _inventoryService, IMapper _mapper)
        : RequestConsumer<UpdateInventoryEvent, UpdateInventoryContract>
    {
        protected override async Task<UpdateInventoryContract> Handle(ConsumeContext<UpdateInventoryEvent> context)
        {
            if (string.IsNullOrWhiteSpace(context.Message.SKUCode))
            {
                throw new InvalidDataException("SKUCode is required for inventory update");
            }

            var updateDto = _mapper.Map<UpdateInventoryDTO>(context.Message);
            await _inventoryService.UpdateBySKUCodeAsync(context.Message.SKUCode, updateDto);

            return new() { IsSuccess = true };
        }
    }
}