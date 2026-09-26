using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace InventorySvc.WebAPI.Consumers
{
    public class CheckQuantityOfListSKUDataConsumer(IInventoryService _inventoryService)
        : RequestConsumer<CheckQuantityOfListSKUDataEvent, CheckQuantityOfListSKUDataContract>
    {
        protected override async Task<CheckQuantityOfListSKUDataContract> Handle(ConsumeContext<CheckQuantityOfListSKUDataEvent> context)
        {
            var skuCodes = context.Message.SKUEventItems
                .Select(item => item.SKUCode
                    ?? throw new InvalidDataException("SKUCode cannot be null or empty"))
                .ToList();

            var quantities = await _inventoryService.GetInventoriesBySKUCodesAsync(skuCodes);

            foreach (var item in context.Message.SKUEventItems)
            {
                var availableQuantity = quantities
                    .FirstOrDefault(q => q.SKUCode == item.SKUCode)?
                    .Quantity ?? 0;

                if (availableQuantity < item.Quantity)
                {
                    throw new ValidationFailureException("Insufficient stock for SKU");
                }
            }

            return new();
        }
    }
}