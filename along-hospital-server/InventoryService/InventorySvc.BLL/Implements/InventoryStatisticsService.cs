using AutoMapper;
using InventorySvc.BLL.DTOs.StatisticsDTOs;
using InventorySvc.BLL.Interfaces;
using InventorySvc.DAL.Models;
using MessageBroker.Contracts.InventoryContracts;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;

namespace InventorySvc.BLL.Implements
{
    public class InventoryStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : IInventoryStatisticsService
    {
        private readonly IGenericRepository<Inventory> _inventoryRepository = unitOfWork.Repository<Inventory>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetInventoryStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var inventoriesQuery = _inventoryRepository.GetAllQueryable()
                .Where(inventory =>
                    inventory.CreationDate >= fromUtc &&
                    inventory.CreationDate < toExclusive);

            var totalInventoryItems = await inventoriesQuery.CountAsync();
            var outOfStockItems = await inventoriesQuery.CountAsync(i => i.Quantity == 0);
            var lowStockItems = await inventoriesQuery.CountAsync(i => i.MinQuantity > 0 && i.Quantity > 0 && i.Quantity <= i.MinQuantity);
            var normalItems = totalInventoryItems - outOfStockItems - lowStockItems;
            var totalQuantity = await inventoriesQuery.SumAsync(i => (int?)i.Quantity) ?? 0;

            var inventoryByDay = await inventoriesQuery
                .GroupBy(inventory => inventory.CreationDate.Date)
                .Select(group => new { group.Key, Count = group.Count() })
                .ToDictionaryAsync(
                    item => DateOnly.FromDateTime(item.Key),
                    item => item.Count);

            var statistics = new InventoryStatisticsDTO
            {
                TotalInventoryItems = totalInventoryItems,
                LowStockItems = lowStockItems,
                OutOfStockItems = outOfStockItems,
                TotalQuantity = totalQuantity,
                StockStatus = StatisticsContractBuilder.CreateDistribution(
                    [
                        ("OutOfStock", outOfStockItems),
                        ("LowStock", lowStockItems),
                        ("Normal", normalItems)
                    ]),
                InventoryOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(
                    fromDate, toDate, "inventories", inventoryByDay)
            };

            return _mapper.Map<GetInventoryStatisticsByDateRangeContract>(statistics);
        }
    }
}
