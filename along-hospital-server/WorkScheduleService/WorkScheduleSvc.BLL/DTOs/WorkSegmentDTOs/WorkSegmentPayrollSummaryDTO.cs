namespace WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs
{
    public class WorkSegmentPayrollSummaryDTO
    {
        public int TotalWorkedMinutes { get; set; }

        public int OvertimeMinutes { get; set; }

        public int LateMinutes { get; set; }

        public int EarlyLeaveMinutes { get; set; }
    }
}