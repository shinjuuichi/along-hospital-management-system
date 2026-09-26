using PayrollSvc.BLL.DTOs.AllowanceDTOs;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IAllowanceService : IBaseCrudService<CreateAllowanceDTO, UpdateAllowanceDTO, GetAllowanceDTO>;
}