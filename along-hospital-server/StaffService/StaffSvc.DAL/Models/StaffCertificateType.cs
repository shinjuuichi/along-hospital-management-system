using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace StaffSvc.DAL.Models
{
    public class StaffCertificateType : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(100)]
        [Unique]
        public string Name { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(255)]
        public string ScopeOfPractice { get; set; } = string.Empty;

        public virtual ICollection<StaffCertificate> StaffCertificates { get; set; } = [];
    }
}