using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetMedicineByIdConsumer(
        IMedicineService medicineService,
        IMapper mapper)
            : RequestConsumer<GetMedicineByIdEvent, GetMedicineByIdContract>
    {
        private readonly IMedicineService _medicineService = medicineService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetMedicineByIdContract> Handle(ConsumeContext<GetMedicineByIdEvent> context)
        {
            var medicineDto = await _medicineService.GetByIdAsync(context.Message.Id);

            var medicineContract = _mapper.Map<GetMedicineByIdContract>(medicineDto);
            return medicineContract;
        }
    }
}
