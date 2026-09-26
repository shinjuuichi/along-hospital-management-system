using AutoMapper;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs;
using ReportSvc.BLL.Interfaces;
using ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.Interfaces.ExternalServices;

namespace ReportSvc.BLL.Implements
{
    public class ManagerDashboardService(
        IMapper mapper,
        IExternalOrderStatisticsService externalOrderStatisticsService,
        IExternalMedicalHistoryStatisticsService externalMedicalHistoryStatisticsService,
        IExternalAppointmentStatisticsService externalAppointmentStatisticsService,
        IExternalUserGrowthStatisticsService externalUserGrowthStatisticsService)
        : IManagerDashboardService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IExternalOrderStatisticsService _externalOrderStatisticsService = externalOrderStatisticsService;
        private readonly IExternalMedicalHistoryStatisticsService _externalMedicalHistoryStatisticsService = externalMedicalHistoryStatisticsService;
        private readonly IExternalAppointmentStatisticsService _externalAppointmentStatisticsService = externalAppointmentStatisticsService;
        private readonly IExternalUserGrowthStatisticsService _externalUserGrowthStatisticsService = externalUserGrowthStatisticsService;

        public async Task<ManagerDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            var orderTask = _externalOrderStatisticsService.GetStatisticsAsync(filterDTO);
            var medicalHistoryTask = _externalMedicalHistoryStatisticsService.GetStatisticsAsync(filterDTO);
            var appointmentTask = _externalAppointmentStatisticsService.GetStatisticsAsync(filterDTO);
            var userGrowthTask = _externalUserGrowthStatisticsService.GetStatisticsAsync(filterDTO);

            await Task.WhenAll(orderTask, medicalHistoryTask, appointmentTask, userGrowthTask);

            var aggregate = new ManagerDashboardAggregateDTO
            {
                Order = await orderTask,
                MedicalHistory = await medicalHistoryTask,
                Appointment = await appointmentTask,
                UserGrowth = await userGrowthTask,
            };

            return _mapper.Map<ManagerDashboardStatisticsDTO>(aggregate);
        }
    }
}
