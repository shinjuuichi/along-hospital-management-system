using System.ComponentModel.DataAnnotations.Schema;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using StaffSvc.DAL.Enums;

namespace StaffSvc.DAL.Models
{
    public class StaffContract : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string ContractCode { get; set; } = string.Empty;

        public StaffContractTypeEnum ContractType { get; set; } = StaffContractTypeEnum.Probation;

        public DateOnly StartDate { get; set; }

        [DateValidator(NotBefore = nameof(StartDate))]
        public DateOnly? EndDate { get; set; }

        [NumberHigherThan(0)]
        public double HourlyRate { get; set; }

        [NumberHigherThan(0)]
        public int WorkingHoursPerWeek { get; set; }

        public StaffContractStatusEnum Status { get; set; } = StaffContractStatusEnum.Active;

        public DateOnly? SignedDate { get; set; }

        [MessageMaxLength(255)]
        public string? SignatureImage { get; set; }

        [MessageRange(0, 1), Column(TypeName = "numeric(18,4)")]
        public double InsuranceSalaryRate { get; set; }

        public int StaffId { get; set; }

        public int RegionalWageId { get; set; }

        public virtual Staff? Staff { get; set; }
        public virtual RegionalWage? RegionalWage { get; set; }
    }
}