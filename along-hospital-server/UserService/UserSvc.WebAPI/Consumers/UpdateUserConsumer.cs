using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class UpdateUserConsumer(IUserService _userService, IMapper _mapper) : RequestConsumer<UpdateUserEvent, UpdateUserContract>
    {
        protected override async Task<UpdateUserContract> Handle(ConsumeContext<UpdateUserEvent> context)
        {
            var updateDto = _mapper.Map<UpdateUserDTO>(context.Message);
            var updatedUser = await _userService.UpdateAsync(context.Message.Id, updateDto);
            var response = _mapper.Map<UpdateUserContract>(updatedUser);
            return response;
        }
    }
}