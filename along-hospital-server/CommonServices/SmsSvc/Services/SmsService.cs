using SmsSvc.SmsHandler;

namespace SmsSvc.Services
{
    public class SmsService(ISmsSender smsSender) : ISmsService
    {
        private static readonly string OtpTemplate =
            "Your Along Hospital OTP is: {0}. Expires in 5 minutes.";

        private static readonly string LinkTemplate =
            "Your Along Hospital verification link is: {0}. Expires in 15 minutes.";

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            var message = string.Format(OtpTemplate, otp);
            await smsSender.SendAsync(phoneNumber, message);
        }

        public async Task SendLinkAsync(string phoneNumber, string link)
        {
            var message = string.Format(LinkTemplate, link);
            await smsSender.SendAsync(phoneNumber, message);
        }
    }
}
