using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace SupplierSvc.DAL.Models
{
    public class Import : AuditEntity
    {
        public DateTime ImportDate { get; set; } = DateTime.UtcNow;

        [MessageMaxLength(1000)]
        public string? Note { get; set; }

        [MessageRequired]
        public int ManagerId { get; set; }

        [MessageRequired]
        public int SupplierId { get; set; }

        [MessageRequired]
        public int ImportRequestId { get; set; }

        public virtual ImportRequest? ImportRequest { get; set; }

        public virtual ICollection<ImportDetail> ImportDetails { get; set; } = [];
    }
}