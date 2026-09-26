using SharedLibrary.Base.Services;
using SupplierSvc.BLL.DTOs.ImportDTOs;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.Interfaces
{
    public interface IImportService : IBaseCrudService<CreateImportDTO, UpdateImportDTO, GetImportDTO>
    {
        //Task<BulkImportFromExcelResponseDTO> BulkImportFromExcelAsync(List<ReadImportRowFromExcelDTO> excelRows);
        Task UpdateInventoriesFromImportAsync(Import import);
        Task<GetImportDTO> CreateImportFromApprovedAsync(CreateImportDTO createDTO);
    }
}