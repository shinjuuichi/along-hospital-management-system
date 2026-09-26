using EmailSvc.DTOs;

namespace EmailSvc.Services
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(SendOtpEmailDTO dto);
        Task SendLinkEmailAsync(SendLinkEmailDTO dto);

        Task SendAppointmentReminderEmailAsync(SendAppointmentReminderEmailDTO dto);

        Task SendLowStockEmailAsync(SendLowStockMedicinesEmailDTO dto);

        Task SendFeedbackRespondEmailAsync(SendFeedbackRespondEmailDTO dto);

        Task SendStaffRequestEmailAsync(SendStaffRequestEmailDTO dto);

        Task SendInterviewEmailAsync(SendInterviewEmailDTO dto);
        Task SendInterviewResultEmailAsync(SendInterviewResultEmailDTO dto);

        Task SendContractExpiringEmailAsync(SendContractExpiringEmailDTO dto);
        Task SendCertificateExpirationReminderEmailAsync(SendCertificateExpirationReminderEmailDTO dto);

        Task SendInvoiceEmailAsync(SendInvoiceEmailDTO dto);
    }
}
