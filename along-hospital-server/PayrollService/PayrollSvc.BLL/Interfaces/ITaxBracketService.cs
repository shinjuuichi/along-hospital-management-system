using PayrollSvc.BLL.DTOs.TaxBracketDTOs;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface ITaxBracketService : IBaseCrudService<CreateTaxBracketDTO, UpdateTaxBracketDTO, GetTaxBracketDTO>
    {
        Task<List<GetTaxBracketDTO>> GetCurrentConfigAsync();
    }
}
