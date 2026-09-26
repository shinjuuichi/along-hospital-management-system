using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Contracts.UserContracts;

namespace ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs
{
    internal class ManagerDashboardAggregateDTO
    {
        public GetOrderStatisticsByDateRangeContract? Order { get; set; }
        public GetMedicalHistoryStatisticsByDateRangeContract? MedicalHistory { get; set; }
        public GetAppointmentStatisticsByDateRangeContract? Appointment { get; set; }
        public GetUserGrowthStatisticsByDateRangeContract? UserGrowth { get; set; }
    }
}
