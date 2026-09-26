using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class TerminateStaffInAuthConsumer(IAuthService _authService) : EventConsumer<TerminateStaffEvent>
    {
        protected override async Task Handle(ConsumeContext<TerminateStaffEvent> context)
        {
            await _authService.TerminateAuthAccountsByUserIdsAsync(context.Message.StaffIds);
        }
    }
}
