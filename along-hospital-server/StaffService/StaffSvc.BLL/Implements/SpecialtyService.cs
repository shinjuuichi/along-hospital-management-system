using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.SpecialtyDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class SpecialtyService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Specialty, CreateSpecialtyDTO, UpdateSpecialtyDTO, GetSpecialtyDTO>(unitOfWork, mapper), ISpecialtyService
    {
        public async Task<bool> CheckSpecialtyExistByIdAsync(int specialtyId)
        {
            return await _repository.AnyAsync(s => s.Id == specialtyId);
        }

        public async Task<List<GetSpecialtyDTO>> GetMedicalSpecialtiesAsync()
        {
            var specialties = await _repository.GetAllAsync(s => s.IsMedical == true);
            return _mapper.Map<List<GetSpecialtyDTO>>(specialties);
        }
    }
}
