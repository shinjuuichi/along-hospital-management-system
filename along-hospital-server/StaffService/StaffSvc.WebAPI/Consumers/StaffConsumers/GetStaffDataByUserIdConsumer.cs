using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class GetStaffDataByUserIdConsumer(
        IStaffService staffService,
        IMapper mapper,
        IMessageBus messageBus) : RequestConsumer<GetStaffDataByUserIdEvent, GetStaffDataByUserIdContract>
    {
        private readonly IStaffService _staffService = staffService;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        protected override async Task<GetStaffDataByUserIdContract> Handle(ConsumeContext<GetStaffDataByUserIdEvent> context)
        {
            var getUserContract = await _messageBus.RequestAsync
                <GetUserDataByUserIdEvent, GetUserDataByUserIdContract>(
                    new() { UserId = context.Message.UserId });

            var staffDTO = await _staffService.GetByIdAsync(context.Message.UserId);

            var getStaffContract = _mapper.Map<GetStaffDataByUserIdContract>(getUserContract);
            getStaffContract = getStaffContract with
            {
                SpecialtyName = staffDTO.SpecialtyName,
                QualificationName = staffDTO.QualificationName,
                Status = staffDTO.Status,
                SpecialtyId = staffDTO.SpecialtyId,
                QualificationId = staffDTO.QualificationId,
                BankCode = staffDTO.BankCode,
                AccountNumber = staffDTO.AccountNumber,
                DependentQuantity = staffDTO.DependentQuantity
            };

            return getStaffContract;
        }
    }
}
