using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetListMedicineDataByNamesConsumer(
        IMedicineService medicineService,
        IMapper mapper)
        : RequestConsumer<GetListMedicineDataByNamesEvent, GetListMedicineDataByNamesContract>
    {
        private readonly IMedicineService _medicineService = medicineService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicineDataByNamesContract> Handle(
            ConsumeContext<GetListMedicineDataByNamesEvent> context)
        {
            var names = context.Message.Names;
            var medicines = await _medicineService.GetAllByNamesAsync(names);
            var getMedicineByIdsContract = _mapper.Map<List<GetMedicineByIdContract>>(medicines);

            return new GetListMedicineDataByNamesContract { Data = getMedicineByIdsContract };
        }
    }
}
