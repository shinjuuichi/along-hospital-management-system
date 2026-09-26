using SharedLibrary.Base.Services;
using SupplierSvc.BLL.DTOs.SupplierDTOs;

namespace SupplierSvc.BLL.Interfaces
{
    public interface ISupplierService : IBaseCrudService<UpsertSupplierDTO, UpsertSupplierDTO, GetSupplierDTO>
    {
        Task<GetSupplierDTO> GetByNameAsync(string? supplierName);
        Task<GetSupplierDTO> GetOrCreateByNameAsync(string supplierName);
    }
}