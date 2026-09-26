using MessageBroker.Contracts.AttendanceContracts;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Calculators;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.Calculators
{
    public class WorkSegmentCalculator : IWorkSegmentCalculator
    {
        private const string CheckInLabel = "CheckIn";
        private const string CheckOutLabel = "CheckOut";

        #region Primary Methods
        public List<WorkSegment> BuildSegments(
            GetWorkScheduleAssignmentForSegmentDTO workScheduleAssignmentForSegmentDTO,
            List<AttendanceLogContract> attendanceContracts)
        {
            var shiftStart = this.GetShiftStart(workScheduleAssignmentForSegmentDTO.WorkDate, workScheduleAssignmentForSegmentDTO.ShiftStartTime);
            var shiftEnd = this.GetShiftEnd(
                workScheduleAssignmentForSegmentDTO.WorkDate,
                workScheduleAssignmentForSegmentDTO.ShiftStartTime,
                workScheduleAssignmentForSegmentDTO.ShiftEndTime);
            var attendanceLogs = attendanceContracts
                .Where(x => DateOnly.FromDateTime(x.LogTime) == workScheduleAssignmentForSegmentDTO.WorkDate)
                .ToList();
            var attendanceEvaluation = this.EvaluateAttendance(attendanceLogs, shiftStart, shiftEnd);
            var boundaries = new HashSet<DateTime> { shiftStart, shiftEnd };

            if (attendanceEvaluation.WorkedRange != null)
            {
                boundaries.Add(attendanceEvaluation.WorkedRange.Value.Start);
                boundaries.Add(attendanceEvaluation.WorkedRange.Value.End);
            }

            var orderedBoundaries = boundaries.OrderBy(x => x).ToList();
            var workSegments = new List<WorkSegment>();
            for (var i = 0; i < orderedBoundaries.Count - 1; i++)
            {
                var start = orderedBoundaries[i];
                var end = orderedBoundaries[i + 1];
                if (end <= start)
                {
                    continue;
                }

                var resolution = this.ResolveSegment(
                    start,
                    end,
                    attendanceEvaluation.WorkedRange,
                    attendanceEvaluation.AbsentReason,
                    workScheduleAssignmentForSegmentDTO.IsOvertimeShift == true);

                workSegments.Add(new WorkSegment
                {
                    WorkScheduleAssignmentId = workScheduleAssignmentForSegmentDTO.Id,
                    StartTime = start,
                    EndTime = end,
                    WorkStatus = resolution.Status,
                    WorkStatusReason = resolution.Reason
                });
            }

            return workSegments;
        }

        public WorkSegmentPayrollSummaryDTO CalculatePayrollSummary(List<WorkSegment> workSegments)
        {
            var payrollSummaryDTO = new WorkSegmentPayrollSummaryDTO();
            if (workSegments.Count == 0)
            {
                return payrollSummaryDTO;
            }

            var workedSegments = workSegments
                .Where(x => x.WorkStatus == WorkStatusEnum.Worked)
                .OrderBy(x => x.StartTime)
                .ToList();
            payrollSummaryDTO.OvertimeMinutes = workSegments
                .Where(x => x.WorkStatus == WorkStatusEnum.Overtime)
                .Sum(x => this.GetMinutes(x.StartTime, x.EndTime));
            payrollSummaryDTO.TotalWorkedMinutes = workedSegments
                .Sum(x => this.GetMinutes(x.StartTime, x.EndTime));

            if (workedSegments.Count == 0)
            {
                return payrollSummaryDTO;
            }

            var shiftStart = workSegments.Min(x => x.StartTime);
            var shiftEnd = workSegments.Max(x => x.EndTime);
            var firstWorkedStart = workedSegments.First().StartTime;
            var lastWorkedEnd = workedSegments.Last().EndTime;

            payrollSummaryDTO.LateMinutes = this.GetMinutes(shiftStart, firstWorkedStart);
            payrollSummaryDTO.EarlyLeaveMinutes = this.GetMinutes(lastWorkedEnd, shiftEnd);

            return payrollSummaryDTO;
        }
        #endregion

        #region Helper Methods
        private DateTime GetShiftStart(DateOnly workDate, TimeOnly startTime)
        {
            return workDate.ToDateTime(startTime);
        }

        private DateTime GetShiftEnd(DateOnly workDate, TimeOnly startTime, TimeOnly endTime)
        {
            var shiftEnd = workDate.ToDateTime(endTime);
            var shiftStart = workDate.ToDateTime(startTime);
            if (shiftEnd <= shiftStart)
            {
                shiftEnd = shiftEnd.AddDays(1);
            }

            return shiftEnd;
        }

        private AttendanceEvaluation EvaluateAttendance(List<AttendanceLogContract> attendanceContracts, DateTime shiftStart, DateTime shiftEnd)
        {
            var checkIns = attendanceContracts
                .Where(x => string.Equals(x.LogType, CheckInLabel, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.LogTime)
                .ToList();
            var checkOuts = attendanceContracts
                .Where(x => string.Equals(x.LogType, CheckOutLabel, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.LogTime)
                .ToList();

            if (checkIns.Count == 0 && checkOuts.Count == 0)
            {
                return new AttendanceEvaluation(null, WorkStatusReasonEnum.NoAttendanceLog);
            }

            if (checkIns.Count == 0)
            {
                return new AttendanceEvaluation(null, WorkStatusReasonEnum.MissingCheckIn);
            }

            if (checkOuts.Count == 0)
            {
                return new AttendanceEvaluation(null, WorkStatusReasonEnum.MissingCheckOut);
            }

            var start = checkIns.Min();
            var end = checkOuts.Max();
            if (end <= start || end <= shiftStart || start >= shiftEnd)
            {
                return new AttendanceEvaluation(null, WorkStatusReasonEnum.InvalidAttendanceRange);
            }

            start = start < shiftStart ? shiftStart : start;
            end = end > shiftEnd ? shiftEnd : end;

            if (end <= start)
            {
                return new AttendanceEvaluation(null, WorkStatusReasonEnum.InvalidAttendanceRange);
            }

            return new AttendanceEvaluation(new TimeRange(start, end), WorkStatusReasonEnum.WorkedFromAttendance);
        }

        private SegmentResolution ResolveSegment(
            DateTime segmentStart,
            DateTime segmentEnd,
            TimeRange? workedRange,
            WorkStatusReasonEnum absentReason,
            bool isOvertimeShift)
        {
            if (workedRange != null
                && segmentStart >= workedRange.Value.Start
                && segmentEnd <= workedRange.Value.End)
            {
                return new SegmentResolution(
                    isOvertimeShift ? WorkStatusEnum.Overtime : WorkStatusEnum.Worked,
                    WorkStatusReasonEnum.WorkedFromAttendance);
            }

            if (workedRange != null)
            {
                if (segmentEnd <= workedRange.Value.Start)
                {
                    return new SegmentResolution(WorkStatusEnum.Absent, WorkStatusReasonEnum.LateArrival);
                }

                if (segmentStart >= workedRange.Value.End)
                {
                    return new SegmentResolution(WorkStatusEnum.Absent, WorkStatusReasonEnum.EarlyLeave);
                }
            }

            return new SegmentResolution(WorkStatusEnum.Absent, absentReason);
        }

        private int GetMinutes(DateTime start, DateTime end)
        {
            var duration = end - start;
            return duration.TotalMinutes <= 0 ? 0 : (int)Math.Round(duration.TotalMinutes);
        }

        private readonly record struct AttendanceEvaluation(TimeRange? WorkedRange, WorkStatusReasonEnum AbsentReason);
        private readonly record struct SegmentResolution(WorkStatusEnum Status, WorkStatusReasonEnum Reason);
        private readonly record struct TimeRange(DateTime Start, DateTime End);
        #endregion
    }
}
