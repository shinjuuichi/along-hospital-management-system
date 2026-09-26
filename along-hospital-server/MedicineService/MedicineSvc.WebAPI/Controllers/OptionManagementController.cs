using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.OptionDTOs;
using MedicineSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicineSvc.WebAPI.Controllers
{
    public class OptionManagementController(IOptionService optionService)
        : CrudController<CreateOptionDTO, UpdateOptionDTO, GetOptionDTO, OptionFilterDTO>(optionService)
    {
        protected override string? EntityName => "Option";
    }
}