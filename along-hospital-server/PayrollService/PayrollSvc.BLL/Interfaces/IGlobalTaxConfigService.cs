using PayrollSvc.BLL.DTOs.GlobalTaxConfigDTOs;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IGlobalTaxConfigService : IBaseCrudService<CreateGlobalTaxConfigDTO, UpdateGlobalTaxConfigDTO, GetGlobalTaxConfigDTO>
    {
        Task<GetGlobalTaxConfigDTO> GetCurrentConfigAsync();
    }
}