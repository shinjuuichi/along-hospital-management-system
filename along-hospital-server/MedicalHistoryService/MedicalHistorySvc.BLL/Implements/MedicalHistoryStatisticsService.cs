using AutoMapper;
using MedicalHistorySvc.BLL.DTOs.StatisticsDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using MessageBroker.Contracts.MedicalHistoryContracts;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;

namespace MedicalHistorySvc.BLL.Implements
{
    public class MedicalHistoryStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : IMedicalHistoryStatisticsService
    {
        private readonly IGenericRepository<MedicalHistory> _medicalHistoryRepository = unitOfWork.Repository<MedicalHistory>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetMedicalHistoryStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var medicalHistories = await _medicalHistoryRepository.GetAllAsync(
                medicalHistory =>
                    medicalHistory.AdmissionDate >= fromUtc &&
                    medicalHistory.AdmissionDate < toExclusive);

            var admissionsByDay = medicalHistories
                .GroupBy(medicalHistory => DateOnly.FromDateTime(medicalHistory.AdmissionDate))
                .ToDictionary(group => group.Key, group => group.Count());

            var statistics = new MedicalHistoryStatisticsDTO
            {
                TotalMedicalHistories = medicalHistories.Count,
                InpatientHistories = medicalHistories.Count(medicalHistory => medicalHistory.MedicalHistoryType == MedicalHistoryTypeEnum.Inpatient),
                OutpatientHistories = medicalHistories.Count(medicalHistory => medicalHistory.MedicalHistoryType == MedicalHistoryTypeEnum.Outpatient),
                PendingPaymentHistories = medicalHistories.Count(medicalHistory => medicalHistory.MedicalHistoryStatus == MedicalHistoryStatusEnum.PendingPayment),
                CompletedHistories = medicalHistories.Count(medicalHistory => medicalHistory.MedicalHistoryStatus == MedicalHistoryStatusEnum.Completed),
                MedicalHistoryStatus = StatisticsContractBuilder.CreateDistribution(
                    medicalHistories.GroupBy(medicalHistory => medicalHistory.MedicalHistoryStatus)
                        .ToDictionary(group => group.Key, group => group.Count())),
                MedicalHistoryType = StatisticsContractBuilder.CreateDistribution(
                    medicalHistories.GroupBy(medicalHistory => medicalHistory.MedicalHistoryType)
                        .ToDictionary(group => group.Key, group => group.Count())),
                AdmissionsOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(fromDate, toDate, "admissions", admissionsByDay)
            };

            return _mapper.Map<GetMedicalHistoryStatisticsByDateRangeContract>(statistics);
        }
    }
}
