using WorkScheduleSvc.BLL.DTOs;

namespace WorkScheduleSvc.BLL.Interfaces.Gateways
{
    public interface IWorkScheduleAssignmentGateway
    {
        Task<GetStaffDTO> GetStaffAsync(int staffId);
        Task<List<GetStaffDTO>> GetStaffsAsync(List<int> staffIds);
        Task<GetRoomDTO> GetRoomAsync(int roomId);
        Task<GetTeleRoomDTO> GetTeleRoomAsync(int teleRoomId);
        Task<List<GetStaffDTO>> GetListStaffDataByRoleAsync();
        Task<List<GetStaffDTO>> GetDoctorStaffDataAsync(int? specialtyId = null);
    }
}
