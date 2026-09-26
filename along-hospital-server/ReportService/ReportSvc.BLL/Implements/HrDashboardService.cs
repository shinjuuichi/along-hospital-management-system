using AutoMapper;
using ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces;
using ReportSvc.BLL.Interfaces.ExternalServices;

namespace ReportSvc.BLL.Implements
{
    public class HrDashboardService(
        IMapper mapper,
        IExternalRecruitmentStatisticsService externalRecruitmentStatisticsService,
        IExternalStaffRequestStatisticsService externalStaffRequestStatisticsService)
        : IHrDashboardService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IExternalRecruitmentStatisticsService _externalRecruitment = externalRecruitmentStatisticsService;
        private readonly IExternalStaffRequestStatisticsService _externalStaffRequest = externalStaffRequestStatisticsService;

        public async Task<HrDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            var recruitmentTask = _externalRecruitment.GetStatisticsAsync(filterDTO);
            var staffRequestTask = _externalStaffRequest.GetStatisticsAsync(filterDTO);

            await Task.WhenAll(recruitmentTask, staffRequestTask);

            var recruitmentResult = await recruitmentTask;
            var staffRequestResult = await staffRequestTask;

            var recruitmentDto = _mapper.Map<HrRecruitmentStatisticsDTO>(recruitmentResult);
            recruitmentDto.InterviewPassRate = CalculateInterviewPassRate(recruitmentResult.PassedApplications, recruitmentResult.FailedApplications);

            var leaveDto = _mapper.Map<HrLeaveStatisticsDTO>(staffRequestResult);

            return _mapper.Map<HrDashboardStatisticsDTO>(new HrDashboardAggregateDTO
            {
                Recruitment = recruitmentDto,
                Leave = leaveDto
            });
        }

        private static double CalculateInterviewPassRate(int passed, int failed)
        {
            var total = passed + failed;
            return total == 0 ? 0 : Math.Round((double)passed / total * 100, 2);
        }
    }
}