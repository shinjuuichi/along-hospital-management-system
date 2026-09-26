using AutoMapper;
using MessageBroker.Contracts.UserContracts;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Enums;
using SharedLibrary.Utils;
using UserSvc.BLL.DTOs.StatisticsDTOs;
using UserSvc.BLL.Interfaces;
using UserSvc.DAL.Models;

namespace UserSvc.BLL.Implements
{
    public class UserStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : IUserStatisticsService
    {
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetUserGrowthStatisticsByDateRangeContract> GetUserGrowthStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var users = await _userRepository.GetAllAsync(
                user =>
                    user.CreationDate >= fromUtc &&
                    user.CreationDate < toExclusive);

            var newPatients = users.Where(user => user.Role == RoleEnum.Patient).ToList();
            var newStaff = users.Where(IsStaffRole).ToList();

            var patientSeries = newPatients
                .GroupBy(user => DateOnly.FromDateTime(user.CreationDate))
                .ToDictionary(group => group.Key, group => group.Count());
            var staffSeries = newStaff
                .GroupBy(user => DateOnly.FromDateTime(user.CreationDate))
                .ToDictionary(group => group.Key, group => group.Count());

            var statistics = new UserGrowthStatisticsDTO
            {
                NewPatients = newPatients.Count,
                NewStaff = newStaff.Count,
                NewUsersOverTime = StatisticsContractBuilder.CreateMultiSeriesChart(
                    fromDate,
                    toDate,
                    ("patient", patientSeries),
                    ("staff", staffSeries)),
                NewStaffRoleDistribution = StatisticsContractBuilder.CreateDistribution(
                    newStaff.GroupBy(user => user.Role)
                        .Select(group => (group.Key.ToString(), group.Count())))
            };

            return _mapper.Map<GetUserGrowthStatisticsByDateRangeContract>(statistics);
        }

        private static bool IsStaffRole(User user)
        {
            return user.Role != RoleEnum.Patient;
        }
    }
}
