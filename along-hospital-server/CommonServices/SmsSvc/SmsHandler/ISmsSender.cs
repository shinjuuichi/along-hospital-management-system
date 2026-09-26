namespace SmsSvc.SmsHandler
{
    public interface ISmsSender
    {
        Task SendAsync(string toPhone, string content, CancellationToken cancellationToken = default);
    }
}
