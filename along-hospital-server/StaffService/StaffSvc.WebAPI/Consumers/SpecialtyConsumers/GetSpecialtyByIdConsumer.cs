using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.SpecialtyConsumers
{
    public class GetSpecialtyByIdConsumer(
        ISpecialtyService specialtyService,
        IMapper mapper) :
        RequestConsumer<GetSpecialtyByIdEvent, GetSpecialtyByIdContract>
    {
        private readonly ISpecialtyService _specialtyService = specialtyService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetSpecialtyByIdContract> Handle(ConsumeContext<GetSpecialtyByIdEvent> context)
        {
            var specialty = await _specialtyService.GetByIdAsync(context.Message.SpecialtyId);
            var specialtyContract = _mapper.Map<GetSpecialtyByIdContract>(specialty);

            return specialtyContract;
        }
    }
}