using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Enums;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class CreateUserToAuthConsumer(
        IUserService userService,
        IMapper mapper,
        IMessageBus messageBus)
        : RequestConsumer<CreateUserToAuthEvent, CreateUserToAuthContract>
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        protected override async Task<CreateUserToAuthContract> Handle(ConsumeContext<CreateUserToAuthEvent> context)
        {
            var createUserDto = _mapper.Map<CreateUserDTO>(context.Message);
            createUserDto.Role ??= nameof(RoleEnum.Patient);
            var userDto = await _userService.CreateAsync(createUserDto);

            try
            {
                var createAuthEvent = _mapper.Map<CreateAuthEvent>(context.Message);
                createAuthEvent = createAuthEvent with { UserId = userDto.Id };

                await _messageBus.RequestAsync<CreateAuthEvent, CreateAuthContract>(createAuthEvent);
            }
            catch
            {
                await _userService.DeleteAsync(userDto.Id);
                throw;
            }

            return new CreateUserToAuthContract
            {
                IsSuccess = true,
                Id = userDto.Id
            };
        }
    }
}