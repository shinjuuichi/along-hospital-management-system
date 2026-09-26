using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using StaffSvc.DAL.Enums;

namespace StaffSvc.DAL.Models
{
    public class StaffCertificate : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string CertificateNo { get; set; } = string.Empty;

        [MessageRequired]
        [DateValidator(AllowFuture = false)]
        public DateOnly IssuedDate { get; set; }

        [MessageRequired]
        [DateValidator(NotBefore = nameof(IssuedDate))]
        public DateOnly ExpiredDate { get; set; }

        [MessageRequired]
        [MessageMaxLength(255)]
        public string IssuedBy { get; set; } = string.Empty;

        public StaffCertificateStatusEnum Status { get; set; } = StaffCertificateStatusEnum.Valid;

        [MessageMaxLength(500)]
        public string? Reason { get; set; }

        [MessageRequired]
        public int StaffCertificateTypeId { get; set; }

        [MessageRequired]
        public int StaffId { get; set; }

        public virtual StaffCertificateType? StaffCertificateType { get; set; }
        public virtual Staff? Staff { get; set; }
    }
}