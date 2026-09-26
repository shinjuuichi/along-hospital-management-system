using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportRequestDTOs
{
    public class CreateImportRequestDTO : MapTo<ImportRequest>
    {
        public List<CreateImportRequestDetailDTO> Details { get; set; } = [];
    }
}