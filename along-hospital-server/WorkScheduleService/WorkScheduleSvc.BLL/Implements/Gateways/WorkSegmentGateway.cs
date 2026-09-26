using MessageBroker.Contracts.AttendanceContracts;
using MessageBroker.Events.AttendanceEvents;
using MessageBroker.Events.PayrollEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Gateways;

namespace WorkScheduleSvc.BLL.Implements.Gateways
{
    public class WorkSegmentGateway(IMessageBus messageBus) : IWorkSegmentGateway
    {
        private readonly IMessageBus _messageBus = messageBus;

        #region Public Methods
        public async Task<Dictionary<int, List<AttendanceLogContract>>> GetAttendanceLogsAsync(List<int> staffIds, WorkSegmentPeriodDTO periodDTO)
        {
            if (staffIds.Count == 0)
            {
                return new Dictionary<int, List<AttendanceLogContract>>();
            }

            var attendanceContract = await _messageBus.RequestAsync<GetAttendanceByStaffsAndRangeEvent, GetAttendanceByStaffsAndRangeContract>(
                new GetAttendanceByStaffsAndRangeEvent
                {
                    StaffIds = staffIds,
                    FromDate = periodDTO.PeriodStart,
                    ToDate = periodDTO.PeriodEnd
                });

            return attendanceContract.Data
                .GroupBy(x => x.StaffId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.LogTime).ToList());
        }

        public async Task PublishPayrollAsync(int staffId, WorkSegmentPayrollSummaryDTO payrollSummaryDTO)
        {
            await _messageBus.PublishAsync(new CreatePayrollEvent
            {
                StaffId = staffId,
                TotalWorkedMinutes = payrollSummaryDTO.TotalWorkedMinutes,
                OvertimeMinutes = payrollSummaryDTO.OvertimeMinutes,
                LateMinutes = payrollSummaryDTO.LateMinutes,
                EarlyLeaveMinutes = payrollSummaryDTO.EarlyLeaveMinutes
            });
        }
        #endregion
    }
}
