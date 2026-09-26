using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using StaffSvc.BLL.DTOs.StaffAccountDTOs;
using StaffSvc.DAL.Enums;

namespace StaffSvc.BLL.Interfaces
{
    public interface IStaffService : IBaseCrudService<CreateStaffAndAccountDTO, UpdateStaffAndAccountDTO, GetStaffAndAccountDTO>
    {
        Task CreateStaffAndAccountAsync(CreateStaffAndAccountDTO createStaffAndAccount);
        Task UpdateStaffAndAccountAsync(int staffId, UpdateStaffAndAccountDTO updateStaffAndAccountDTO);
        Task<PaginationResult<GetStaffProfileDTO>> GetStaffsByRoleAsync(string role, FilterDTO filterDTO);
        Task UpdateStatusAsync(List<int> staffIds, StaffStatusEnum newStatus);
        Task<bool> CheckExistByIdAsync(int id);
        Task<bool> CheckExistByIdsAsync(List<int> ids);
        Task<List<int>> GetActiveStaffIdsByIdsAsync(List<int> ids);
    }
}
