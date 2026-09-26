using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportRequestDTOs
{
    public class CreateImportRequestDetailDTO : MapTo<ImportRequestDetail>
    {
        public string? SKUCode { get; set; }

        public int RequestQuantity { get; set; }
    }
}