using ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs;

namespace ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs.Statistics
{
    public class ManagerDashboardStatisticsDTO
    {
        public ManagerDashboardOverviewDTO? Overview { get; set; }
        public ManagerOrderStatisticsDTO? Order { get; set; }
        public ManagerMedicalHistoryStatisticsDTO? MedicalHistory { get; set; }
        public ManagerAppointmentStatisticsDTO? Appointment { get; set; }
        public ManagerUserGrowthStatisticsDTO? UserGrowth { get; set; }
    }
}
