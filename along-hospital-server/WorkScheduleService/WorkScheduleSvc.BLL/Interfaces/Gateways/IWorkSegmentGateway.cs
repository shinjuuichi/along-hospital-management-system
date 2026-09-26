using MessageBroker.Contracts.AttendanceContracts;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;

namespace WorkScheduleSvc.BLL.Interfaces.Gateways
{
    public interface IWorkSegmentGateway
    {
        Task<Dictionary<int, List<AttendanceLogContract>>> GetAttendanceLogsAsync(List<int> staffIds, WorkSegmentPeriodDTO periodDTO);
        Task PublishPayrollAsync(int staffId, WorkSegmentPayrollSummaryDTO payrollSummaryDTO);
    }
}
