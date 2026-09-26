using AutoMapper;
using MessageBroker.Contracts.StaffRequestContracts;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;
using StaffRequestSvc.BLL.DTOs.StatisticsDTOs;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.Implements;

public class StaffRequestStatisticsService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IStaffRequestStatisticsService
{
    private readonly IMapper _mapper = mapper;
    private readonly IGenericRepository<LeaveRequest> _leaveRequestRepository = unitOfWork.Repository<LeaveRequest>();
    private readonly IGenericRepository<SalaryAdvance> _salaryAdvanceRepository = unitOfWork.Repository<SalaryAdvance>();

    public async Task<GetStaffRequestStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
    {
        var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
        var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

        var leaveRequests = await _leaveRequestRepository.GetAllAsync(
            request =>
                request.CreationDate >= fromUtc &&
                request.CreationDate < toExclusive);
        var salaryAdvances = await _salaryAdvanceRepository.GetAllAsync(
            request =>
                !request.IsDeleted &&
                request.CreationDate >= fromUtc &&
                request.CreationDate < toExclusive);

        var statistics = new StaffRequestStatisticsDTO
        {
            PendingLeaveRequests = leaveRequests.Count(request => request.Status == RequestStatusEnum.Pending),
            ApprovedLeaveRequests = leaveRequests.Count(request => request.Status == RequestStatusEnum.Approved),
            RejectedLeaveRequests = leaveRequests.Count(request => request.Status == RequestStatusEnum.Rejected),
            PendingSalaryAdvances = salaryAdvances.Count(request => request.Status == SalaryAdvanceStatusEnum.Pending),
            ApprovedSalaryAdvances = salaryAdvances.Count(request =>
                request.Status == SalaryAdvanceStatusEnum.Approved ||
                request.Status == SalaryAdvanceStatusEnum.Disbursed),
            DisbursedSalaryAdvances = salaryAdvances.Count(request => request.Status == SalaryAdvanceStatusEnum.Disbursed),
            SalaryAdvanceAmountApproved = salaryAdvances
                .Where(request =>
                    request.Status == SalaryAdvanceStatusEnum.Approved ||
                    request.Status == SalaryAdvanceStatusEnum.Disbursed)
                .Sum(request => request.Amount),
            LeaveRequestStatus = StatisticsContractBuilder.CreateDistribution(
                leaveRequests.GroupBy(request => request.Status)
                    .ToDictionary(group => group.Key, group => group.Count())),
            SalaryAdvanceStatus = StatisticsContractBuilder.CreateDistribution(
                salaryAdvances.GroupBy(request => request.Status)
                    .ToDictionary(group => group.Key, group => group.Count()))
        };

        return _mapper.Map<GetStaffRequestStatisticsByDateRangeContract>(statistics);
    }
}
