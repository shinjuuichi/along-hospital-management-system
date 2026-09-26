using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportRequestDTOs
{
    public class GetImportRequestDetailDTO : MapFrom<ImportRequestDetail>
    {
        public int Id { get; set; }

        public string? SKUCode { get; set; }

        public int RequestQuantity { get; set; }

        public string? MedicineName { get; set; }

        public double? UnitPrice { get; set; }
    }
}