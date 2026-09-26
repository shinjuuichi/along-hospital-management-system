using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers;

public class GetUserEmailsByRolesConsumer(IUserService _userService, IMapper _mapper)
    : RequestConsumer<GetUserEmailsByRolesEvent, GetUserEmailsByRolesContract>
{
    protected override async Task<GetUserEmailsByRolesContract> Handle(ConsumeContext<GetUserEmailsByRolesEvent> context)
    {
        var message = context.Message;

        var userEmails = await _userService.GetUserEmailsByRolesAsync(message.Roles, message.ExcludeUserIds);

        var mappedUserEmails = _mapper.Map<List<UserEmailData>>(userEmails);
        return new GetUserEmailsByRolesContract
        {
            Data = mappedUserEmails
        };
    }
}
