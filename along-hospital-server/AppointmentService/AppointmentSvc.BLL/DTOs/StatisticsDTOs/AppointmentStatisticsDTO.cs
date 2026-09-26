using MessageBroker.Contracts.StatisticsContracts;

namespace AppointmentSvc.BLL.DTOs.StatisticsDTOs
{
    public class AppointmentStatisticsDTO
    {
        public int Appointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public StatisticsDistributionContract AppointmentStatus { get; set; } = new();
        public StatisticsDistributionContract AppointmentMeetingType { get; set; } = new();
        public StatisticsDistributionContract AppointmentPaymentStatus { get; set; } = new();
        public StatisticsChartContract AppointmentsOverTime { get; set; } = new();
    }
}
