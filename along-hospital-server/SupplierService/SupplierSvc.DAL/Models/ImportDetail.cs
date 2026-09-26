using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace SupplierSvc.DAL.Models
{
    [PrimaryKey(nameof(ImportId), nameof(SKUCode))]
    public class ImportDetail : Entity
    {
        public int ImportId { get; set; }

        public virtual Import? Import { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        public string SKUCode { get; set; } = string.Empty;

        [NumberPositive]
        public int Quantity { get; set; }

        [NumberPositive]
        public double UnitPrice { get; set; }
    }
}