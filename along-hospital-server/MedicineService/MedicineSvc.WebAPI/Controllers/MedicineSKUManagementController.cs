using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using MedicineSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicineSvc.WebAPI.Controllers
{
    public class MedicineSKUManagementController(IMedicineSKUService medicineSkuService)
        : CrudController<CreateMedicineSKUDTO, UpdateMedicineSKUDTO, GetMedicineSKUDTO, MedicineSKUFilterDTO>(medicineSkuService)
    {
        protected override string? EntityName => "Medicine SKU";
    }
}