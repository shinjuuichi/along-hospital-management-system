using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.OptionDTOs;
using MedicineSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicineSvc.WebAPI.Controllers
{
    public class OptionValueManagementController(IOptionValueService optionValueService)
        : CrudController<CreateOptionValueDTO, UpdateOptionValueDTO, GetOptionValueDTO, OptionValueFilterDTO>(optionValueService)
    {
        protected override string? EntityName => "Option Value";
    }
}