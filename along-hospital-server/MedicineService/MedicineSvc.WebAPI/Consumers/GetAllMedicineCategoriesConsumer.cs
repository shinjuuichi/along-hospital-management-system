using AutoMapper;
using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicineSvc.WebAPI.Consumers
{
    public class GetAllMedicineCategoriesConsumer(IMedicineCategoryService medicineCategoryService, IMapper mapper)
        : RequestConsumer<GetAllMedicineCategoriesEvent, GetAllMedicineCategoriesContract>
    {
        private readonly IMedicineCategoryService _medicineCategoryService = medicineCategoryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAllMedicineCategoriesContract> Handle(ConsumeContext<GetAllMedicineCategoriesEvent> context)
        {
            var medicineCategories = await _medicineCategoryService.GetAllAsync();
            var medicineCategoriesContractItems = _mapper.Map<List<GetAllMedicineCategoriesContractItem>>(medicineCategories);
            return new GetAllMedicineCategoriesContract
            {
                MedicineCategories = medicineCategoriesContractItems
            };
        }
    }
}
