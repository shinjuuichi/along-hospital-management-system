using AppointmentSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace AppointmentSvc.DAL.Models
{
    public class Appointment : AuditEntity
    {
        [MessageRequired]
        public DateOnly Date { get; set; }

        [MessageMaxLength(1000)]
        public string? Purpose { get; set; }

        public AppointmentStatusEnum AppointmentStatus { get; set; } = AppointmentStatusEnum.Scheduled;

        public AppointmentMeetingTypeEnum AppointmentMeetingType { get; set; } = AppointmentMeetingTypeEnum.InPerson;

        public AppointmentPaymentStatusEnum AppointmentPaymentStatus { get; set; } = AppointmentPaymentStatusEnum.Pending;

        public DateTime? CompletedDate { get; set; }

        public DateTime? CancelledDate { get; set; }

        public Guid? TransactionId { get; set; }

        public int? MedicalHistoryId { get; set; }

        [MessageRequired]
        public int SpecialtyId { get; set; }

        [MessageRequired]
        public int PatientId { get; set; }

        [MessageRequired]
        public int TimeSlotId { get; set; }

        public virtual TimeSlot? TimeSlot { get; set; }

        [JsonColumn]
        public virtual TimeSlotSnapshot? TimeSlotSnapshot { get; set; }
    }
}