using MedicineSvc.BLL.DTOs.MedicineUnitDTOs;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Interfaces
{
    public interface IMedicineUnitService
        : IBaseCrudService<CreateMedicineUnitDTO, UpdateMedicineUnitDTO, GetMedicineUnitDTO>;
}
