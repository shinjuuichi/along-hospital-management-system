using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SupplierSvc.DAL.Enums;

namespace SupplierSvc.DAL.Models
{
    public class ImportRequest : AuditEntity
    {
        public DateOnly RequestDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public ImportRequestStatusEnum Status { get; set; } = ImportRequestStatusEnum.Pending;

        public int? ApprovedByUserId { get; set; }

        public virtual ICollection<ImportRequestDetail> ImportRequestDetails { get; set; } = [];

        public virtual Import? Import { get; set; }
    }
}