using SharedLibrary.Commons;
using SharedLibrary.Utils;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SmsSvc.SmsHandler
{
    public class TwilioSmsSender(
        AppConfiguration configuration,
        ILogger<TwilioSmsSender> logger) : ISmsSender
    {
        public async Task SendAsync(string toPhone, string content, CancellationToken cancellationToken = default)
        {
            var normalizedPhone = StringUtil.NormalizePhone(toPhone);

            TwilioClient.Init(configuration.TwilioConfig.AccountSid, configuration.TwilioConfig.AuthToken);

            try
            {
                var response = await MessageResource.CreateAsync(
                    to: new PhoneNumber("+18777804236"), // Use a test phone number for testing
                    body: content,
                    messagingServiceSid: configuration.TwilioConfig.MessageSid);

                logger.LogInformation("Sent SMS via Twilio to {Phone}. MessageSid: {MessageSid}", normalizedPhone, response.Sid);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred while sending SMS to {Phone} via Twilio", normalizedPhone);
                throw;
            }
        }
    }
}
