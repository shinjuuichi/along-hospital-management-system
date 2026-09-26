using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetListMedicineDataBySKUCodesConsumer(
        IMedicineService medicineService,
        IMapper mapper)
        : RequestConsumer<GetListMedicineDataBySKUCodesEvent, GetListMedicineSKUDataContract>
    {
        private readonly IMedicineService _medicineService = medicineService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicineSKUDataContract> Handle(
            ConsumeContext<GetListMedicineDataBySKUCodesEvent> context)
        {
            var skuCodes = context.Message.SKUCodes;
            var skus = await _medicineService.GetAllBySKUCodesAsync(skuCodes);
            var skuContracts = _mapper.Map<List<GetMedicineSKUContract>>(skus);

            return new() { Data = skuContracts };
        }
    }
}