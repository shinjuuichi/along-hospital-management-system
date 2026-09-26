using EmailSvc.DTOs;
using Microsoft.AspNetCore.Identity.UI.Services;
using SharedLibrary.Commons;
using System.Net;

namespace EmailSvc.Services
{
    public class EmailService(IEmailSender _emailSender, IEmailTemplateService _templateService, AppConfiguration _appConfig) : IEmailService
    {
        public async Task SendOtpEmailAsync(SendOtpEmailDTO dto)
        {
            const string templateName = "send-otp-email";
            var htmlBody = await _templateService.RenderTemplateAsync(templateName, parameters: dto.Otp);
            await _emailSender.SendEmailAsync(dto.Email, dto.Subject, htmlBody);
        }

        public async Task SendLinkEmailAsync(SendLinkEmailDTO dto)
        {
            const string templateName = "send-link-email";
            var htmlBody = await _templateService.RenderTemplateAsync(templateName, parameters: dto.Link);
            await _emailSender.SendEmailAsync(dto.Email, dto.Subject, htmlBody);
        }

        public async Task SendAppointmentReminderEmailAsync(SendAppointmentReminderEmailDTO dto)
        {
            const string templateName = "send-appointment-reminder-email";
            var subject = $"Appointment Reminder · #{dto.AppointmentId}";

            var placeholders = new Dictionary<string, string?>
            {
                ["Id"] = dto.AppointmentId.ToString(),
                ["Date"] = dto.Date.ToString("yyyy-MM-dd"),
                ["Time"] = dto.Time.ToString("HH:mm"),
                ["SpecialtyName"] = dto.SpecialtyName,
                ["PatientName"] = dto.PatientName,
                ["LinkToAppointmentUrl"] = $"{_appConfig.UrlsConfig?.FrontendUrl}/patient/appointments/{dto.AppointmentId}",
                ["TimeZoneId"] = TimeZoneInfo.Local.Id,
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email, subject, htmlBody);
        }

        public async Task SendLowStockEmailAsync(SendLowStockMedicinesEmailDTO dto)
        {
            const string templateName = "send-low-stock-email";

            var medicinesHtml = string.Join("", dto.Medicines.Select(m => $@"
                <tr style='border-bottom: 1px solid #e2e8f0;'>
                    <td style='padding: 16px 12px; font-size: 14px; color: #0f172a;'>
                        <div style='font-weight: 600;'>{m.Name}</div>
                        <div style='font-size: 12px; color: #64748b; margin-top: 4px;'>ID: {m.MedicineId}</div>
                    </td>
                    <td style='padding: 16px 12px; font-size: 14px; color: #475569;'>{m.Brand ?? "N/A"}</td>
                    <td style='padding: 16px 12px; font-size: 14px; color: #475569;'>{m.CategoryName ?? "N/A"}</td>
                    <td style='padding: 16px 12px; font-size: 14px; text-align: center;'>
                        <span style='display: inline-block; padding: 6px 12px; background: {(m.Quantity == 0 ? "#fee2e2" : "#fef3c7")}; color: {(m.Quantity == 0 ? "#dc2626" : "#d97706")}; border-radius: 6px; font-weight: 600;'>{m.Quantity}</span>
                    </td>
                    <td style='padding: 16px 12px; font-size: 14px; color: #475569; text-align: center;'>{m.MinQuantity ?? 0}</td>
                    <td style='padding: 16px 12px; font-size: 14px; color: #475569;'>{m.MedicineUnit ?? "N/A"}</td>
                    <td style='padding: 16px 12px; font-size: 12px; color: #64748b;'>{(m.LastImportDate.HasValue ? m.LastImportDate.Value.ToString("dd/MM/yyyy") : "N/A")}</td>
                </tr>
            "));

            var placeholders = new Dictionary<string, string?>
            {
                ["TotalCount"] = dto.Medicines.Count.ToString(),
                ["MedicinesRows"] = medicinesHtml,
                ["GeneratedDate"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ["DashboardUrl"] = $"{_appConfig.UrlsConfig?.FrontendUrl}/manager"
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email, dto.Subject, htmlBody);
        }

        public async Task SendFeedbackRespondEmailAsync(SendFeedbackRespondEmailDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return;
            }

            const string templateName = "send-feedback-respond-email";
            const string subject = "Along Hospital - New response to your feedback";

            var encodedContent = WebUtility.HtmlEncode(dto.Content ?? string.Empty)
                .Replace("\r\n", "<br />")
                .Replace("\n", "<br />");

            var placeholders = new Dictionary<string, string?>
            {
                ["StaffName"] = string.IsNullOrWhiteSpace(dto.StaffName) ? "Along Hospital Staff" : dto.StaffName,
                ["RespondContent"] = string.IsNullOrWhiteSpace(encodedContent)
                    ? "No details were provided in this response."
                    : encodedContent
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email, subject, htmlBody);
        }

        public async Task SendStaffRequestEmailAsync(SendStaffRequestEmailDTO dto)
        {
            const string templateName = "send-staff-request-email";

            var actionColor = dto.Action switch
            {
                "Approved" => "#16a34a",
                "Rejected" => "#dc2626",
                "Canceled" => "#d97706",
                _ => "#1e88e5"
            };

            var actionIcon = dto.Action switch
            {
                "Approved" => "&#10004;",
                "Rejected" => "&#10008;",
                "Canceled" => "&#9888;",
                _ => "&#10010;"
            };

            var reasonSection = string.IsNullOrWhiteSpace(dto.Reason)
                ? string.Empty
                : $@"<div style='margin-top: 16px; padding: 14px 18px; background: #f0f4ff; border-left: 4px solid #1e88e5; border-radius: 6px;'>
                        <div style='font-size: 12px; font-weight: 600; color: #1e88e5;'>Reason</div>
                        <div style='font-size: 14px; color: #0f172a; margin-top: 6px;'>{WebUtility.HtmlEncode(dto.Reason)}</div>
                     </div>";

            var deciderSection = string.IsNullOrWhiteSpace(dto.DeciderName)
                ? string.Empty
                : $@"<div class='row'>
                        <span class='label'>Decided By:&nbsp;</span>
                        <span class='value'>{WebUtility.HtmlEncode(dto.DeciderName)}</span>
                     </div>";

            var placeholders = new Dictionary<string, string?>
            {
                ["StaffName"] = dto.StaffName,
                ["DeciderSection"] = deciderSection,
                ["RequestType"] = dto.RequestType,
                ["Action"] = dto.Action,
                ["ActionColor"] = actionColor,
                ["ActionIcon"] = actionIcon,
                ["Details"] = dto.Details,
                ["ReasonSection"] = reasonSection,
                ["GeneratedDate"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            var subject = $"Along Hospital - {dto.RequestType} {dto.Action}";

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);

            foreach (var email in dto.RecipientEmails)
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    await _emailSender.SendEmailAsync(email, subject, htmlBody);
                }
            }
        }

        public async Task SendInterviewEmailAsync(SendInterviewEmailDTO dto)
        {
            const string templateName = "send-interview-email";
            var subject = $"Along Hospital - Interview Invitation for {dto.JobTitle}";

            var placeholders = new Dictionary<string, string?>
            {
                ["CandidateName"] = dto.CandidateName,
                ["Email"] = dto.Email,
                ["Phone"] = dto.Phone,
                ["ApplyDate"] = dto.ApplyDate.ToString("yyyy-MM-dd"),
                ["JobTitle"] = dto.JobTitle,
                ["InterviewDate"] = dto.InterviewDate.ToString("yyyy-MM-dd HH:mm"),
                ["Note"] = dto.Note,
                ["InterviewTypeName"] = dto.InterviewTypeName,
                ["InterviewTypeDescription"] = dto.InterviewTypeDescription,
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email!, subject, htmlBody);
        }

        public async Task SendInterviewResultEmailAsync(SendInterviewResultEmailDTO dto)
        {
            const string templateName = "send-interview-result-email";
            var isPassed = dto.Result?.Equals("Passed", StringComparison.OrdinalIgnoreCase) == true;
            var resultClass = isPassed ? "passed" : "failed";
            var subject = "Along Hospital - Kết quả phỏng vấn";

            var placeholders = new Dictionary<string, string?>
            {
                ["CandidateName"] = dto.CandidateName,
                ["ApplyDate"] = dto.ApplyDate.ToString("yyyy-MM-dd"),
                ["JobTitle"] = dto.JobTitle,
                ["InterviewDate"] = dto.InterviewDate.ToString("yyyy-MM-dd HH:mm"),
                ["Result"] = dto.Result,
                ["ApplicationStatus"] = dto.ApplicationStatus,
                ["ResultClass"] = resultClass,
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email!, subject, htmlBody);
        }

        public async Task SendContractExpiringEmailAsync(SendContractExpiringEmailDTO dto)
        {
            const string templateName = "send-contract-expiring-email";
            var subject = $"Contract Expiring Soon - {dto.DaysRemaining} Days Remaining";

            var placeholders = new Dictionary<string, string?>
            {
                ["StaffName"] = dto.StaffName ?? "Staff",
                ["ContractEndDate"] = dto.ContractEndDate.ToString("dd/MM/yyyy"),
                ["DaysRemaining"] = dto.DaysRemaining.ToString()
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email!, subject, htmlBody);
        }

        public async Task SendCertificateExpirationReminderEmailAsync(SendCertificateExpirationReminderEmailDTO dto)
        {
            const string templateName = "send-certificate-expiration-reminder-email";

            var subject = $"Certificate Expiration Reminder · {dto.CertificateName}";

            var urgentStyle = dto.DaysUntilExpiration <= 7
                ? "background: #fee2e2; color: #dc2626; font-weight: 700;"
                : dto.DaysUntilExpiration <= 14
                    ? "background: #fef3c7; color: #d97706; font-weight: 600;"
                    : "background: #dbeafe; color: #1e40af;";

            var placeholders = new Dictionary<string, string?>
            {
                ["StaffName"] = dto.StaffName,
                ["CertificateName"] = dto.CertificateName,
                ["CertificateNo"] = dto.CertificateNo,
                ["ExpiredDate"] = dto.ExpiredDate.ToString("dd/MM/yyyy"),
                ["DaysUntilExpiration"] = dto.DaysUntilExpiration.ToString(),
                ["UrgentStyle"] = urgentStyle
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email!, subject, htmlBody);
        }

        public async Task SendInvoiceEmailAsync(SendInvoiceEmailDTO dto)
        {
            const string templateName = "send-invoice-email";
            var invoiceUrl = $"{_appConfig.UrlsConfig?.FrontendUrl}/invoice/print/{dto.InvoiceId}";
            var subject = $"Along Hospital - Invoice {dto.InvoiceNumber}";
            var invoiceRowsVi = dto.LineItems.Count == 0
                ? """
                    <tr>
                        <td colspan="5" style="padding: 16px 12px; text-align: center; color: #64748b; font-size: 14px;">
                            Chưa có chi tiết dịch vụ
                        </td>
                    </tr>
                    """
                : string.Join("", dto.LineItems.Select((item, index) =>
                {
                    var serviceName = WebUtility.HtmlEncode(item.ServiceName ?? "Service");
                    var serviceDescription = WebUtility.HtmlEncode(item.ServiceDescription ?? "No description");

                    return $"""
                        <tr style="border-bottom: 1px solid #e2e8f0;">
                            <td style="padding: 14px 12px; font-size: 14px; color: #0f172a; text-align: center;">{index + 1}</td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #0f172a;">
                                <div style="font-weight: 600;">{serviceName}</div>
                                <div style="font-size: 12px; color: #64748b; margin-top: 4px;">{serviceDescription}</div>
                            </td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #475569; text-align: center;">x{item.Quantity}</td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #475569; text-align: right;">${item.UnitPrice}</td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #0f172a; font-weight: 600; text-align: right;">${item.TotalAmount}</td>
                        </tr>
                        """;
                }));
            var invoiceRowsEn = dto.LineItems.Count == 0
                ? """
                    <tr>
                        <td colspan="5" style="padding: 16px 12px; text-align: center; color: #64748b; font-size: 14px;">
                            No service details available
                        </td>
                    </tr>
                    """
                : string.Join("", dto.LineItems.Select((item, index) =>
                {
                    var serviceName = WebUtility.HtmlEncode(item.ServiceName ?? "Service");
                    var serviceDescription = WebUtility.HtmlEncode(item.ServiceDescription ?? "No description");

                    return $"""
                        <tr style="border-bottom: 1px solid #e2e8f0;">
                            <td style="padding: 14px 12px; font-size: 14px; color: #0f172a; text-align: center;">{index + 1}</td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #0f172a;">
                                <div style="font-weight: 600;">{serviceName}</div>
                                <div style="font-size: 12px; color: #64748b; margin-top: 4px;">{serviceDescription}</div>
                            </td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #475569; text-align: center;">x{item.Quantity}</td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #475569; text-align: right;">${item.UnitPrice}</td>
                            <td style="padding: 14px 12px; font-size: 14px; color: #0f172a; font-weight: 600; text-align: right;">${item.TotalAmount}</td>
                        </tr>
                        """;
                }));

            var placeholders = new Dictionary<string, string?>
            {
                ["PatientName"] = WebUtility.HtmlEncode(dto.PatientName),
                ["InvoiceNumber"] = WebUtility.HtmlEncode(dto.InvoiceNumber),
                ["MedicalHistoryNumber"] = WebUtility.HtmlEncode(dto.MedicalHistoryNumber ?? $"HSBA-{dto.MedicalHistoryId}"),
                ["CreationDate"] = dto.CreationDate.ToString("dd/MM/yyyy HH:mm"),
                ["PaymentDate"] = dto.PaymentDate.HasValue ? dto.PaymentDate.Value.ToString("dd/MM/yyyy HH:mm") : "-",
                ["TotalInvoiceAmount"] = $"${dto.TotalInvoiceAmount}",
                ["TotalAmount"] = $"${dto.TotalAmount}",
                ["TotalServices"] = dto.LineItems.Sum(item => item.Quantity).ToString(),
                ["InvoiceRowsVi"] = invoiceRowsVi,
                ["InvoiceRowsEn"] = invoiceRowsEn,
                ["InvoiceUrl"] = invoiceUrl
            };

            var htmlBody = await _templateService.RenderTemplateAsync(templateName, placeholders);
            await _emailSender.SendEmailAsync(dto.Email!, subject, htmlBody);
        }
    }
}
