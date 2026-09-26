using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicineSvc.WebAPI.Controllers
{
    public class MedicineManagementController(IMedicineService medicineService)
        : CrudController<CreateMedicineDTO, UpdateMedicineAndInventoryDTO, GetMedicineDTO, MedicineFilterDTO>(medicineService)
    {
        protected override string? EntityName => "Medicine";
    }
}
