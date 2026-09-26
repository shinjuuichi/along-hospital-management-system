using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class DeleteInventoryConsumer(IInventoryService _inventoryService)
        : RequestConsumer<DeleteInventoryEvent, DeleteInventoryContract>
    {
        protected override async Task<DeleteInventoryContract> Handle(ConsumeContext<DeleteInventoryEvent> context)
        {
            if (string.IsNullOrWhiteSpace(context.Message.SKUCode))
            {
                throw new InvalidDataException("SKUCode cannot be null or empty.");
            }

            await _inventoryService.DeleteBySKUCodeAsync(context.Message.SKUCode);
            return new DeleteInventoryContract { IsSuccess = true };
        }
    }
}