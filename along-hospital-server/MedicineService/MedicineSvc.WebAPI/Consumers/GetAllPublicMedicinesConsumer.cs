using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetAllPublicMedicinesConsumer(IMedicineService medicineService, IMapper mapper)
        : RequestConsumer<GetAllPublicMedicinesEvent, GetAllPublicMedicinesContract>
    {
        private readonly IMedicineService _medicineService = medicineService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAllPublicMedicinesContract> Handle(ConsumeContext<GetAllPublicMedicinesEvent> context)
        {
            var medicineDTOs = await _medicineService.GetAllAsync();
            medicineDTOs = medicineDTOs.Where(m => m.IsPublic).ToList();

            var medicineContractItem = _mapper.Map<List<GetAllMedicinesContractItem>>(medicineDTOs);
            return new GetAllPublicMedicinesContract
            {
                Medicines = medicineContractItem
            };
        }
    }
}
