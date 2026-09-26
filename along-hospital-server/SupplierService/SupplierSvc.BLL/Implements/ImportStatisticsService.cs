using AutoMapper;
using MessageBroker.Contracts.SupplierContracts;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;
using SupplierSvc.BLL.DTOs.StatisticsDTOs;
using SupplierSvc.BLL.Interfaces;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.Implements
{
    public class ImportStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : IImportStatisticsService
    {
        private readonly IGenericRepository<Import> _importRepository = unitOfWork.Repository<Import>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetImportStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var importsQuery = _importRepository.GetAllQueryable()
                .Where(import =>
                    import.ImportDate >= fromUtc &&
                    import.ImportDate < toExclusive);

            var totalImports = await importsQuery.CountAsync();

            var totalImportValue = await importsQuery
                .SelectMany(import => import.ImportDetails)
                .SumAsync(detail => (double?)detail.Quantity * detail.UnitPrice) ?? 0d;

            var importsByDay = await importsQuery
                .GroupBy(import => import.ImportDate.Date)
                .Select(group => new { group.Key, Count = group.Count() })
                .ToDictionaryAsync(
                    item => DateOnly.FromDateTime(item.Key),
                    item => item.Count);

            var importBySupplier = await importsQuery
                .GroupBy(import => import.SupplierId)
                .Select(group => new { SupplierId = group.Key, Count = group.Count() })
                .ToListAsync();

            var importBySupplierDistribution = importBySupplier
                .Select(item => (Label: $"Supplier_{item.SupplierId}", Value: item.Count))
                .ToList();

            var statistics = new ImportStatisticsDTO
            {
                TotalImports = totalImports,
                TotalImportValue = totalImportValue,
                ImportsOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(
                    fromDate, toDate, "imports", importsByDay),
                ImportBySupplier = StatisticsContractBuilder.CreateDistribution(importBySupplierDistribution)
            };

            return _mapper.Map<GetImportStatisticsByDateRangeContract>(statistics);
        }
    }
}
