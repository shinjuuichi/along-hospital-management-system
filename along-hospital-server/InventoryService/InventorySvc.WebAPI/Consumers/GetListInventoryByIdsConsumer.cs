using AutoMapper;
using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class GetListInventoryBySKUCodesConsumer(
        IInventoryService inventoryService,
        IMapper mapper)
        : RequestConsumer<GetListInventoryDataBySKUCodesEvent, GetListInventoryDataBySKUCodesContract>
    {
        private readonly IInventoryService _inventoryService = inventoryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListInventoryDataBySKUCodesContract> Handle(ConsumeContext<GetListInventoryDataBySKUCodesEvent> context)
        {
            var skuCodes = context.Message.SKUCodes;
            var inventoryDTOs = await _inventoryService.GetInventoriesBySKUCodesAsync(skuCodes);

            var getInventoryBySKUCodesContract = _mapper
                .Map<List<GetInventoryBySKUCodeContract>>(inventoryDTOs);
            return new() { Data = getInventoryBySKUCodesContract };
        }
    }
}