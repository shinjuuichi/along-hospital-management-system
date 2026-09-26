using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using SharedLibrary.Commons;

namespace EmailSvc.EmailHandler
{
    public class EmailSender(ISendGridClient client, AppConfiguration configuration, ILogger<EmailSender> logger) : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            var from = new EmailAddress(configuration.EmailConfig.Email, configuration.EmailConfig.DisplayName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlBody);

            try
            {
                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    logger.LogError("Failed to send email to {Email}. Status: {StatusCode}, Response: {Response}",
                        email, response.StatusCode, responseBody);
                }
                else
                {
                    logger.LogInformation("Email sent successfully to {Email}. Subject: {Subject}. Content: {htmlBody}", email, subject, htmlBody);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {Email}. Subject: {Subject}. Content: {htmlBody}", email, subject, htmlBody);
            }
        }
    }
}
