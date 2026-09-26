using MedicalServiceSvc.BLL.DTOs;
using SharedLibrary.Base.Services;

namespace MedicalServiceSvc.BLL.Interfaces
{
    public interface IMedicalServiceService
        : IBaseCrudService<UpsertMedicalServiceDTO, UpsertMedicalServiceDTO, GetMedicalServiceDTO>
    {
        Task<GetMedicalServiceDTO> GetByCodeAsync(string code);
        Task<List<GetMedicalServiceDTO>> GetAllByCodesAsync(List<string> codes);
        Task<List<GetMedicalServiceDTO>> GetAllForCurrentUserAsync();
    }
}