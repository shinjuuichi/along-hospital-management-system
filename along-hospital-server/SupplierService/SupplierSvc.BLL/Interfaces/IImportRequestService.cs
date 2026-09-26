using SharedLibrary.Base.Services;
using SupplierSvc.BLL.DTOs.ImportRequestDTOs;
using SupplierSvc.DAL.Enums;

namespace SupplierSvc.BLL.Interfaces
{
    public interface IImportRequestService : IBaseCrudService<CreateImportRequestDTO, UpdateImportRequestDTO, GetImportRequestDTO>
    {
        Task ChangeStatusAsync(int id, ImportRequestStatusEnum targetStatus);
    }
}