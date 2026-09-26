using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetListMedicineDataByIdsConsumer(
        IMedicineService medicineService,
        IMapper mapper)
        : RequestConsumer<GetListMedicineDataByIdsEvent, GetListMedicineDataByIdsContract>
    {
        private readonly IMedicineService _medicineService = medicineService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicineDataByIdsContract> Handle(
            ConsumeContext<GetListMedicineDataByIdsEvent> context)
        {
            var ids = context.Message.Ids;
            var getMedicineDTOs = await _medicineService.GetAllByIdsAsync(ids);
            var getMedicineByIdsContract = _mapper.Map<List<GetMedicineByIdContract>>(getMedicineDTOs);

            return new() { Data = getMedicineByIdsContract };
        }
    }
}