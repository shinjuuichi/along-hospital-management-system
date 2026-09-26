using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Interfaces
{
    public interface IMedicineSKUService
        : IBaseCrudService<CreateMedicineSKUDTO, UpdateMedicineSKUDTO, GetMedicineSKUDTO>
    {
        Task<bool> CheckExistByIdAsync(int medicineSkuId);
        Task<bool> CheckExistBySKUCodeAsync(string skuCode);
        Task<List<GetMedicineSKUDTO>> GetAllBySKUCodesAsync(List<string> skuCodes);
        Task<List<GetMedicineSKUDTO>> GetAllPublicBySKUCodesAsync(List<string> skuCodes);
    }
}
