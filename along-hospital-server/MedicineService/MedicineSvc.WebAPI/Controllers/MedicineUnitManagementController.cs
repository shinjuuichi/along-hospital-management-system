using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.MedicineUnitDTOs;
using MedicineSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicineSvc.WebAPI.Controllers
{
    public class MedicineUnitManagementController(IMedicineUnitService medicineUnitService)
        : CrudController<CreateMedicineUnitDTO, UpdateMedicineUnitDTO, GetMedicineUnitDTO, MedicineUnitFilterDTO>(medicineUnitService)
    {
        protected override string? EntityName => "Medicine Unit";
    }
}
