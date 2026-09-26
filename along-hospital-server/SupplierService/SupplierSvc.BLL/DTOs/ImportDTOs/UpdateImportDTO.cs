using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportDTOs
{
    public class UpdateImportDTO : MapTo<Import>
    {
        public string? Note { get; set; }
    }
}