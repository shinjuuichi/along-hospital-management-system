using SharedLibrary.Base.Services;
using UserSvc.BLL.DTOs;

namespace UserSvc.BLL.Interfaces
{
    public interface IUserService : IBaseCrudService<CreateUserDTO, UpdateUserDTO, GetUserDTO>
    {
        Task<UserProfileDTO> GetUserProfileAsync();
        Task UpdateProfileAsync(UpdateProfileUserDTO dto);
        Task CompleteProfileAsync(CreateUserWithRolePatientProfileDTO dto);
        Task<List<int>> GetListUserIdByNameContainsAndRoleAsync(string? name, string? role);
        Task<List<int>> GetUserIdsByFilterAsync(UserFilterRequestDTO userFilterRequestDTO);
        Task<List<UserProfileDTO>> GetListUserDataByRoleAsync(string role);
        Task<List<UserEmailDTO>> GetUserEmailsByRolesAsync(List<string> roles, List<int> excludeUserIds);
        Task<List<GetUserDTO>> GetListUserDataByListRoleAsync(List<string> roles);
        Task<int> GetNewUsersCountByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}