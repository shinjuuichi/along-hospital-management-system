using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Interfaces
{
    public interface IMedicineService
        : IBaseCrudService<CreateMedicineDTO, UpdateMedicineAndInventoryDTO, GetMedicineDTO>
    {
        Task<bool> CheckExistByIdAsync(int medicineId);
        Task<List<GetMedicineDTO>> GetAllByNamesAsync(List<string> names);
        Task<List<GetMedicineSKUDTO>> GetAllBySKUCodesAsync(List<string> skuCodes);
        Task<List<CreatedMedicineFromExcelDTO>> CreateMedicinesFromExcelAsync(List<CreateMedicineFromExcelEventItem> medicines);
        Task<List<GetMedicineDTO>> GetAllInfusionMedicinesAsync();
    }
}