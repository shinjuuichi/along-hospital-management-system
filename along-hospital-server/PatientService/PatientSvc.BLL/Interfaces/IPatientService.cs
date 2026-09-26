using PatientSvc.BLL.DTOs;
using SharedLibrary.Base.Services;

namespace PatientSvc.BLL.Interfaces
{
    public interface IPatientService : IBaseCrudService<CreatePatientDTO, UpdatePatientDTO, GetPatientDTO>
    {
        Task<GetPatientDTO> CreateWithAccountAsync(CreatePatientAndAccountDTO createPatientAndAccountDTO);
        Task<GetPatientDTO> UpdateWithAccountAsync(int id, UpdatePatientAndAccountDTO updatePatientAndAccountDTO);
        Task<GetPatientProfileDTO> GetProfileByPatientIdAsync(int patientId);
        Task<bool> CheckExistByIdAsync(int patientId);
    }
}