using MedicineSvc.BLL.DTOs.OptionDTOs;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Interfaces
{
    public interface IOptionValueService
        : IBaseCrudService<CreateOptionValueDTO, UpdateOptionValueDTO, GetOptionValueDTO>;
}
