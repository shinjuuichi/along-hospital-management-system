using PayrollSvc.BLL.DTOs.DeductionDTOs;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IDeductionService : IBaseCrudService<CreateDeductionDTO, UpdateDeductionDTO, GetDeductionDTO>;
}