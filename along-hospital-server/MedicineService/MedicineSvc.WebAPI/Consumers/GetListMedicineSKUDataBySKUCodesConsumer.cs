using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetListMedicineSKUDataBySKUCodesConsumer(
        IMedicineSKUService medicineSkuService,
        IMapper mapper)
        : RequestConsumer<GetListMedicineSKUDataBySKUCodesEvent, GetListMedicineSKUDataContract>
    {
        private readonly IMedicineSKUService _medicineSkuService = medicineSkuService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicineSKUDataContract> Handle(
            ConsumeContext<GetListMedicineSKUDataBySKUCodesEvent> context)
        {
            var skuCodes = context.Message.SKUCodes;
            var getMedicineSkuDTOs = await _medicineSkuService.GetAllBySKUCodesAsync(skuCodes);
            var contracts = _mapper.Map<List<GetMedicineSKUContract>>(getMedicineSkuDTOs);

            return new() { Data = contracts };
        }
    }
}
