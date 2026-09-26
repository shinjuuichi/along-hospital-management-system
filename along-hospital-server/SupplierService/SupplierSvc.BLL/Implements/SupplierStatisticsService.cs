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
    public class SupplierStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : ISupplierStatisticsService
    {
        private readonly IGenericRepository<Supplier> _supplierRepository = unitOfWork.Repository<Supplier>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetSupplierStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var suppliersQuery = _supplierRepository.GetAllQueryable()
                .Where(supplier =>
                    supplier.CreationDate >= fromUtc &&
                    supplier.CreationDate < toExclusive);

            var totalSuppliers = await suppliersQuery.CountAsync();

            var suppliersByDay = await suppliersQuery
                .GroupBy(supplier => supplier.CreationDate.Date)
                .Select(group => new { group.Key, Count = group.Count() })
                .ToDictionaryAsync(
                    item => DateOnly.FromDateTime(item.Key),
                    item => item.Count);

            var statistics = new SupplierStatisticsDTO
            {
                TotalSuppliers = totalSuppliers,
                SuppliersOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(
                    fromDate, toDate, "suppliers", suppliersByDay)
            };

            return _mapper.Map<GetSupplierStatisticsByDateRangeContract>(statistics);
        }
    }
}
