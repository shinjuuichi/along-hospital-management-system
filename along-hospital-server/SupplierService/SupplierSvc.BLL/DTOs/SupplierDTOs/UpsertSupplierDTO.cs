using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.SupplierDTOs
{
    public class UpsertSupplierDTO : MapTo<Supplier>
    {
        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Note { get; set; }
    }
}