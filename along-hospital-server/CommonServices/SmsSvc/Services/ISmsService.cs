namespace SmsSvc.Services
{
    public interface ISmsService
    {
        Task SendOtpAsync(string phoneNumber, string otp);
        Task SendLinkAsync(string phoneNumber, string link);
    }
}
