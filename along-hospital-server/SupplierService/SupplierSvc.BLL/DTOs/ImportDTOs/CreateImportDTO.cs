using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportDTOs
{
    public class CreateImportDTO : MapTo<Import>
    {
        public int ImportRequestId { get; set; }

        public int SupplierId { get; set; }

        public string? Note { get; set; }

        public List<UpsertImportDetailDTO> Details { get; set; } = [];
    }
}