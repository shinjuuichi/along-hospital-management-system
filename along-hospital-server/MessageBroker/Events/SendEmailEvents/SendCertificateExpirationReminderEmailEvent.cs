using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendCertificateExpirationReminderEmailEvent : BaseEvent
    {
        public string? Email { get; set; }

        public string? StaffName { get; set; }

        public string? CertificateName { get; set; }

        public string? CertificateNo { get; set; }

        public DateOnly ExpiredDate { get; set; }

        public int DaysUntilExpiration { get; set; }
    }
}
