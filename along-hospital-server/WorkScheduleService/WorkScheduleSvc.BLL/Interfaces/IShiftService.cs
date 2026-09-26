using SharedLibrary.Base.Services;
using WorkScheduleSvc.BLL.DTOs.ShiftDTOs;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IShiftService : IBaseCrudService<CreateShiftDTO, UpdateShiftDTO, GetShiftDTO>;
}
