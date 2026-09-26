using AutoMapper;
using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class CreateInventoryConsumer(IInventoryService _inventoryService, IMapper _mapper)
        : RequestConsumer<CreateInventoryEvent, CreateInventoryContract>
    {
        protected override async Task<CreateInventoryContract> Handle(ConsumeContext<CreateInventoryEvent> context)
        {
            var createInventory = _mapper.Map<CreateInventoryDTO>(context.Message);
            var created = await _inventoryService.CreateAsync(createInventory);
            return _mapper.Map<CreateInventoryContract>(created);
        }
    }
}