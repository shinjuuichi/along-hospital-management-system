using MessageBroker.Contracts.AttendanceContracts;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Interfaces.Calculators
{
    public interface IWorkSegmentCalculator
    {
        List<WorkSegment> BuildSegments(
            GetWorkScheduleAssignmentForSegmentDTO assignmentDTO,
            List<AttendanceLogContract> attendanceContracts);
        WorkSegmentPayrollSummaryDTO CalculatePayrollSummary(List<WorkSegment> workSegments);
    }
}
