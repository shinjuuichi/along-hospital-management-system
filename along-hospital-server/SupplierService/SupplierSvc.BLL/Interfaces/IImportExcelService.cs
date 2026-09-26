using SupplierSvc.BLL.DTOs.ImportExcelDTOs;

namespace SupplierSvc.BLL.Interfaces
{
    public interface IImportExcelService
    {
        Task<List<ReadImportRowFromExcelDTO>> ReadImportRowsFromExcelAsync(Stream fileStream, string fileName);
    }
}