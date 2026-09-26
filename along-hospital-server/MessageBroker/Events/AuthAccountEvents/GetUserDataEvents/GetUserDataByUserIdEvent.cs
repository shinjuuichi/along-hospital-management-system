namespace MessageBroker.Events.AuthAccountEvents.GetUserDataEvents
{
    public record GetUserDataByUserIdEvent : GetAuthDataByUserIdEvent;

    public record GetListUserDataByUserIdsEvent : GetListAuthDataByUserIdsEvent;
}