using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.SpecialtyDTOs;

namespace StaffSvc.BLL.Interfaces
{
    public interface ISpecialtyService : IBaseCrudService<CreateSpecialtyDTO, UpdateSpecialtyDTO, GetSpecialtyDTO>
    {
        Task<bool> CheckSpecialtyExistByIdAsync(int specialtyId);
        Task<List<GetSpecialtyDTO>> GetMedicalSpecialtiesAsync();
    }
}