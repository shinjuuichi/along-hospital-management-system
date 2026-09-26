using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.SpecialtyConsumers
{
    public class GetAllSpecialtiesConsumer(ISpecialtyService specialtyService, IMapper mapper)
        : RequestConsumer<GetAllSpecialtiesEvent, GetAllSpecialtiesContract>
    {
        private readonly ISpecialtyService _specialtyService = specialtyService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAllSpecialtiesContract> Handle(ConsumeContext<GetAllSpecialtiesEvent> context)
        {
            var specialtyDTOs = await _specialtyService.GetAllAsync();
            var specialtyContractItems = _mapper.Map<List<GetAllSpecialtiesContractItem>>(specialtyDTOs);
            return new GetAllSpecialtiesContract
            {
                Specialties = specialtyContractItems
            };
        }
    }
}