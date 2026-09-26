using Microsoft.EntityFrameworkCore;
using PatientSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientSvc.DAL.Models
{
    [Index(nameof(MedicalNumber), IsUnique = true)]
    public class Patient : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MessageRequired]
        public override int Id { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        [MedicalNumberValidator]
        public string MedicalNumber { get; set; } = string.Empty;

        [NumberHigherThanOrEqualTo(30)]
        public int? Height { get; set; }

        [NumberPositive]
        public double? Weight { get; set; }

        public BloodTypeEnum BloodType { get; set; } = BloodTypeEnum.Unknown;

        public virtual ICollection<Allergy> Allergies { get; set; } = [];
    }
}
