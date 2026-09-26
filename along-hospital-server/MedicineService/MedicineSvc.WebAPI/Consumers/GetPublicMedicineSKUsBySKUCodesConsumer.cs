using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetPublicMedicineSKUsBySKUCodesConsumer(
        IMedicineSKUService medicineSkuService,
        IMapper mapper)
        : RequestConsumer<GetPublicMedicineSKUsBySKUCodesEvent, GetListMedicineSKUDataContract>
    {
        private readonly IMedicineSKUService _medicineSkuService = medicineSkuService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicineSKUDataContract> Handle(
            ConsumeContext<GetPublicMedicineSKUsBySKUCodesEvent> context)
        {
            var skuCodes = context.Message.SKUCodes;
            var publicSkus = await _medicineSkuService.GetAllPublicBySKUCodesAsync(skuCodes);

            var contracts = _mapper.Map<List<GetMedicineSKUContract>>(publicSkus);
            return new() { Data = contracts };
        }
    }
}