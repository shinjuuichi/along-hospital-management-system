using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models.Snapshots;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalHistorySvc.DAL.Models
{
    public class MedicalHistory : BaseEntity
    {
        [MessageRequired, MessageMaxLength(50), Unique]
        public string MedicalHistoryNumber { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Diagnosis { get; set; }

        public DateOnly? FollowUpAppointmentDate { get; set; }

        public MedicalHistoryStatusEnum MedicalHistoryStatus { get; set; } = MedicalHistoryStatusEnum.PendingPayment;

        public MedicalHistoryTypeEnum MedicalHistoryType { get; set; } = MedicalHistoryTypeEnum.Outpatient;

        public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;

        public DateTime? DischargeDate { get; set; }

        [MessageRequired]
        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        [MessageRequired]
        public int SpecialtyId { get; set; }

        public virtual Prescription? Prescription { get; set; }
        public virtual Complaint? Complaint { get; set; }

        [JsonColumn]
        public virtual PatientSnapshot? PatientSnapshot { get; set; }
    }
}