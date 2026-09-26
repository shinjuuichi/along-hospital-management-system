using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs.Statistics
{
    public class ManagerAppointmentStatisticsDTO
    {
        public int Appointments { get; set; }
        public DashboardDistributionDTO? AppointmentStatus { get; set; }
        public DashboardChartDTO? AppointmentsOverTime { get; set; }
    }
}
