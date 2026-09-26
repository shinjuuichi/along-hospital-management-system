using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class CreateMedicinesFromExcelConsumer(
        IMedicineService medicineService,
        IMapper mapper)
        : RequestConsumer<CreateMedicinesFromExcelEvent, CreateMedicinesFromExcelContract>
    {
        private readonly IMedicineService _medicineService = medicineService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<CreateMedicinesFromExcelContract> Handle(
            ConsumeContext<CreateMedicinesFromExcelEvent> context)
        {
            var medicinesToCreate = context.Message.CreateMedicineFromExcelEventItems;
            var createdMedicineDTOs = await _medicineService.CreateMedicinesFromExcelAsync(medicinesToCreate);

            var createdMedicineContracts = _mapper.Map<List<CreateMedicineDataContractItem>>(createdMedicineDTOs);

            return new CreateMedicinesFromExcelContract { Data = createdMedicineContracts };
        }
    }
}