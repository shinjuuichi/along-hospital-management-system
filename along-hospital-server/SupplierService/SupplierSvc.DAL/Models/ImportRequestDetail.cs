using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SupplierSvc.DAL.Models.Snapshots;

namespace SupplierSvc.DAL.Models
{
    public class ImportRequestDetail : AuditEntity
    {
        [MessageRequired]
        public int ImportRequestId { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        public string SKUCode { get; set; } = string.Empty;

        [NumberPositive]
        public int RequestQuantity { get; set; }

        [JsonColumn]
        public ImportRequestDetailSnapshot? MedicineSnapshot { get; set; }

        public virtual ImportRequest? ImportRequest { get; set; }
    }
}