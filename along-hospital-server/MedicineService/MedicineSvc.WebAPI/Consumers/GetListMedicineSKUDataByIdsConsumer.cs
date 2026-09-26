using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetListMedicineSKUDataByIdsConsumer(
        IMedicineSKUService medicineSkuService,
        IMapper mapper)
        : RequestConsumer<GetListMedicineSKUDataByIdsEvent, GetListMedicineSKUDataContract>
    {
        private readonly IMedicineSKUService _medicineSkuService = medicineSkuService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicineSKUDataContract> Handle(
            ConsumeContext<GetListMedicineSKUDataByIdsEvent> context)
        {
            var ids = context.Message.Ids;
            var getMedicineSkuDTOs = await _medicineSkuService.GetAllByIdsAsync(ids);
            var getMedicineSkuByIdsContract = _mapper.Map<List<GetMedicineSKUContract>>(getMedicineSkuDTOs);

            return new GetListMedicineSKUDataContract { Data = getMedicineSkuByIdsContract };
        }
    }
}