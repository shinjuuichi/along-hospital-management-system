using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.AppointmentContracts
{
    public record GetAppointmentStatisticsByDateRangeContract : BaseContract
    {
        public int Appointments { get; init; }
        public int CompletedAppointments { get; init; }
        public int CancelledAppointments { get; init; }
        public StatisticsDistributionContract AppointmentStatus { get; init; } = new();
        public StatisticsDistributionContract AppointmentMeetingType { get; init; } = new();
        public StatisticsDistributionContract AppointmentPaymentStatus { get; init; } = new();
        public StatisticsChartContract AppointmentsOverTime { get; init; } = new();
    }
}
