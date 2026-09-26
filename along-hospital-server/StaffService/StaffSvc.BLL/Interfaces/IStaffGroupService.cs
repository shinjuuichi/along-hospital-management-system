using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.StaffGroupDTOs;

namespace StaffSvc.BLL.Interfaces
{
    public interface IStaffGroupService : IBaseCrudService<UpsertStaffGroupDTO, UpsertStaffGroupDTO, GetStaffGroupDTO>
    {
        Task<bool> CheckExistByIdAsync(int id);
    }
}