using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class GetListStaffDataByUserIdsConsumer(
        IStaffService staffService,
        IMapper mapper,
        IMessageBus messageBus)
        : RequestConsumer<GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>
    {
        private readonly IStaffService _staffService = staffService;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        protected override async Task<GetListStaffDataByUserIdsContract> Handle(ConsumeContext<GetListStaffDataByUserIdsEvent> context)
        {
            var userIds = context.Message.UserIds;
            var userContracts = await _messageBus.RequestAsync<
                GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new() { UserIds = userIds });

            var staffDTOs = await _staffService.GetAllByIdsAsync(userIds);
            var staffDictDTOs = staffDTOs.ToDictionary(s => s.Id);

            var staffContracts = _mapper.Map<List<GetStaffDataByUserIdContract>>(userContracts.Data);
            for (int i = 0; i < staffContracts.Count; i++)
            {
                var userId = userContracts.Data.ElementAt(i).UserId;
                if (staffDictDTOs.TryGetValue(userId, out var staffDTO))
                {
                    staffContracts[i] = staffContracts[i] with
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
                }
            }

            return new() { Data = staffContracts };
        }
    }
}
