using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using StaffSvc.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffSvc.DAL.Models
{
    public class Staff : AuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public StaffStatusEnum Status { get; set; } = StaffStatusEnum.Active;

        public BankCodeEnum BankCode { get; set; }

        [MessageMaxLength(100)]
        public string AccountNumber { get; set; } = string.Empty;

        [NumberPositive]
        public int DependentQuantity { get; set; }

        public int SpecialtyId { get; set; }

        public int QualificationId { get; set; }

        public virtual Specialty? Specialty { get; set; }

        public virtual Qualification? Qualification { get; set; }

        public virtual ICollection<StaffGroupMember> StaffGroupMembers { get; set; } = [];
        public virtual ICollection<StaffContract> StaffContracts { get; set; } = [];
        public virtual ICollection<StaffCertificate> StaffCertificates { get; set; } = [];
    }
}