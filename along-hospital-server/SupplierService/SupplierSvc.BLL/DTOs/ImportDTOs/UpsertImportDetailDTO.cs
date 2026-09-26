using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportDTOs
{
    public class UpsertImportDetailDTO : MapTo<ImportDetail>
    {
        public string? SKUCode { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }
    }
}