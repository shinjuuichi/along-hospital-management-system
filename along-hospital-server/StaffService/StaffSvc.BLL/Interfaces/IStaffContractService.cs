using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.StaffContractDTOs;
using StaffSvc.DAL.Enums;

namespace StaffSvc.BLL.Interfaces
{
    public interface IStaffContractService : IBaseCrudService<CreateStaffContractDTO, UpdateStaffContractDTO, GetStaffContractDTO>
    {
        Task<GetStaffContractDTO> GetActiveContractByStaffIdAsync(int staffId);
        Task UpdateStatusAsync(int id, StaffContractStatusEnum newStatus);
        Task RenewAsync(int id, RenewStaffContractDTO restoreDTO);
        Task ExpireContractsAsync();
        Task SendContractExpiringNotificationsAsync(int daysBeforeExpiration = 30);
        Task<GetStaffContractDTO> SignContractAsync(int id, SignStaffContractDTO signDTO);
    }
}
