using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.UpdateAccountContracts;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers;

public class UpdateUserToAuthConsumer(
    IMessageBus messageBus,
    IUserService userService,
    IMapper mapper)
    : RequestConsumer<UpdateUserToAuthEvent, UpdateUserToAuthContract>
{
    protected override async Task<UpdateUserToAuthContract> Handle(ConsumeContext<UpdateUserToAuthEvent> context)
    {
        var updateUserDto = mapper.Map<UpdateUserDTO>(context.Message);
        await messageBus.RequestAsync<UpdateAuthEvent, UpdateAuthContract>(mapper.Map<UpdateAuthEvent>(context.Message));

        await userService.UpdateAsync(context.Message.UserId, updateUserDto);
        return new UpdateUserToAuthContract { IsSuccess = true };
    }
}