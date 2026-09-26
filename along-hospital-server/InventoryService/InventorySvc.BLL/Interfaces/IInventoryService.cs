using InventorySvc.BLL.DTOs;
using SharedLibrary.Base.Services;

namespace InventorySvc.BLL.Interfaces
{
    public interface IInventoryService
        : IBaseCrudService<CreateInventoryDTO, UpdateInventoryDTO, GetInventoryDTO>
    {
        Task<int> GetQuantityBySKUCodeAsync(string skuCode);
        Task<GetInventoryDTO> UpdateBySKUCodeAsync(string skuCode, UpdateInventoryDTO updateDTO);
        Task DeleteBySKUCodeAsync(string skuCode);
        Task<GetInventoryDTO> GetInventoryBySKUCodeAsync(string skuCode);
        Task SendLowStockAlertEmailsAsync();
        Task<List<GetInventoryDTO>> GetInventoriesBySKUCodesAsync(List<string> skuCodes);
    }
}