using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers;

public class SendStaffRequestStatusEmailConsumer(IEmailService _emailService, IMessageBus _bus)
    : EventConsumer<SendStaffRequestStatusEmailEvent>
{
    protected override async Task Handle(ConsumeContext<SendStaffRequestStatusEmailEvent> ctx)
    {
        var message = ctx.Message;

        var userIds = new List<int> { message.StaffId };
        if (message.DeciderId.HasValue)
        {
            userIds.Add(message.DeciderId.Value);
        }

        var usersContract = await _bus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
            new GetListUserDataByUserIdsEvent { UserIds = userIds.Distinct().ToList() });

        var userDataDict = usersContract.Data.ToDictionary(u => u.UserId);

        userDataDict.TryGetValue(message.StaffId, out var staffData);
        var staffName = staffData?.Name ?? "Unknown Staff";
        var staffEmail = staffData?.Email;

        if (string.IsNullOrWhiteSpace(staffEmail))
        {
            return;
        }

        string? deciderName = null;
        if (message.DeciderId.HasValue && userDataDict.TryGetValue(message.DeciderId.Value, out var deciderData))
        {
            deciderName = deciderData.Name;
        }

        var dto = new SendStaffRequestEmailDTO
        {
            RecipientEmails = [staffEmail],
            StaffName = staffName,
            DeciderName = deciderName,
            RequestType = message.RequestType,
            Action = message.Action,
            Details = message.Details,
            Reason = message.Reason
        };

        await _emailService.SendStaffRequestEmailAsync(dto);
    }
}
