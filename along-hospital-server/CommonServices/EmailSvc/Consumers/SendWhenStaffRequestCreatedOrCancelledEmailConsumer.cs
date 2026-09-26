using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Enums;

namespace EmailSvc.Consumers;

public class SendWhenStaffRequestCreatedOrCancelledEmailConsumer(IEmailService _emailService, IMessageBus _bus)
    : EventConsumer<SendWhenStaffRequestCreatedOrCancelledEmailEvent>
{
    protected override async Task Handle(ConsumeContext<SendWhenStaffRequestCreatedOrCancelledEmailEvent> ctx)
    {
        var message = ctx.Message;

        var emailsContract = await _bus.RequestAsync<GetUserEmailsByRolesEvent, GetUserEmailsByRolesContract>(
            new GetUserEmailsByRolesEvent
            {
                Roles = [nameof(RoleEnum.HR), nameof(RoleEnum.Manager)],
                ExcludeUserIds = [message.StaffId]
            });

        var recipientEmails = emailsContract.Data
            .Where(u => !string.IsNullOrWhiteSpace(u.Email))
            .Select(u => u.Email!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var usersContract = await _bus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
            new GetListUserDataByUserIdsEvent { UserIds = [message.StaffId] });

        var staffData = usersContract.Data.FirstOrDefault(u => u.UserId == message.StaffId);
        var staffName = staffData?.Name ?? "Unknown Staff";

        var dto = new SendStaffRequestEmailDTO
        {
            RecipientEmails = recipientEmails,
            StaffName = staffName,
            RequestType = message.RequestType,
            Action = message.Action,
            Details = message.Details,
            Reason = message.Reason
        };

        await _emailService.SendStaffRequestEmailAsync(dto);
    }
}
