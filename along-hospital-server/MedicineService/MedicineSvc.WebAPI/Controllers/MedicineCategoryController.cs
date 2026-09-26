using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.MedicineCategoryDTOs;
using MedicineSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicineSvc.WebAPI.Controllers
{
    public class MedicineCategoryController(IMedicineCategoryService medicineCategoryService)
        : CrudController<UpsertMedicineCategoryDTO, UpsertMedicineCategoryDTO, GetMedicineCategoryDTO, MedicineCategoryFilterDTO>(medicineCategoryService)
    {
        protected override string? EntityName => "Medicine Category";
    }
}
