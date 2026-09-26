using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.SupplierDTOs
{
    public class CreateSupplierFromExcelDTO : MapTo<Supplier>
    {
        public string? Name { get; set; }
    }
}