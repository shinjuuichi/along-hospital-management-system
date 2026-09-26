using MedicineSvc.BLL.DTOs.MedicineUnitOptionDTOs;

namespace MedicineSvc.BLL.Interfaces
{
    public interface IMedicineUnitOptionService
    {
        Task UpdateStatusAsync(UpdateMedicineUnitOptionDTO updateDto);
    }
}