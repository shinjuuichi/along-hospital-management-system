using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.SpecialtyConsumers
{
    public class GetListSpecialtyDataByIdsConsumer(
        ISpecialtyService specialtyService,
        IMapper mapper)
        : RequestConsumer<GetListSpecialtyDataByIdsEvent, GetListSpecialtyDataByIdsContract>
    {
        private readonly ISpecialtyService _specialtyService = specialtyService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListSpecialtyDataByIdsContract> Handle(ConsumeContext<GetListSpecialtyDataByIdsEvent> context)
        {
            var specialties = await _specialtyService.GetAllByIdsAsync([.. context.Message.SpecialtyIds]);
            var specialtyContracts = _mapper.Map<List<GetSpecialtyByIdContract>>(specialties);

            return new GetListSpecialtyDataByIdsContract
            {
                IsSuccess = true,
                Data = specialtyContracts,
            };
        }
    }
}