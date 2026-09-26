using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Results;
using StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.BLL.Interfaces
{
    public interface ISalaryAdvanceService : IBaseCrudService<CreateSalaryAdvanceDTO, UpdateSalaryAdvanceDTO, GetSalaryAdvanceDTO>
    {
        Task<PaginationResult<GetSalaryAdvanceDTO>> GetMyRequestsAsync(SalaryAdvanceFilterDTO filterDTO);
        Task UpdateStatusAsync(int id, SalaryAdvanceStatusEnum status);
        Task UpdateListStatusAsync(List<int> ids, SalaryAdvanceStatusEnum status);
        Task<GetSalaryAdvanceDTO?> GetUndisbursedByStaffIdAsync(int staffId);
        Task<GetSalaryAdvanceDTO?> MarkDisbursedByPayrollAsync(int staffId, int payrollId);
    }
}
