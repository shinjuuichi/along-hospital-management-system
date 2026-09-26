using AutoMapper;
using MessageBroker.Contracts.OrderContracts;
using Microsoft.EntityFrameworkCore;
using OrderSvc.BLL.DTOs;
using OrderSvc.BLL.Interfaces;
using OrderSvc.DAL.Enums;
using OrderSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Utils;

namespace OrderSvc.BLL.Implements
{
    public class OrderStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GetOrderStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            OrderStatusEnum[] revenueStates =
            [
                OrderStatusEnum.Paid,
                OrderStatusEnum.Shipping,
                OrderStatusEnum.Completed
            ];

            var dashboardStates = new[]
            {
                (State: OrderStatusEnum.Unpaid, Key: "unpaid"),
                (State: OrderStatusEnum.Paid, Key: "processing"),
                (State: OrderStatusEnum.Shipping, Key: "shipping"),
                (State: OrderStatusEnum.Completed, Key: "completed"),
                (State: OrderStatusEnum.Cancelled, Key: "cancelled")
            };

            var fromDateUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toDateExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var orders = await _unitOfWork.Repository<Order>()
                .GetAllQueryable()
                .Where(order => order.OrderDate >= fromDateUtc && order.OrderDate < toDateExclusive)
                .Select(order => new
                {
                    order.OrderDate,
                    order.OrderStatus,
                    order.FinalPrice
                })
                .ToListAsync();

            var labels = DashboardStatisticsUtil.BuildDateLabels(fromDate, toDate);
            var orderCountByDate = orders
                .GroupBy(order => DateOnly.FromDateTime(order.OrderDate))
                .ToDictionary(group => group.Key, group => group.Count());
            var revenueByDate = orders
                .Where(order => revenueStates.Contains(order.OrderStatus))
                .GroupBy(order => DateOnly.FromDateTime(order.OrderDate))
                .ToDictionary(group => group.Key, group => group.Sum(order => order.FinalPrice));
            var orderCountByState = orders
                .GroupBy(order => order.OrderStatus)
                .ToDictionary(group => group.Key, group => group.Count());

            var statistics = new OrderStatisticsDTO
            {
                Revenue = orders
                    .Where(order => revenueStates.Contains(order.OrderStatus))
                    .Sum(order => order.FinalPrice),
                Orders = orders.Count,
                PendingOrders = orderCountByState.GetValueOrDefault(OrderStatusEnum.Unpaid, 0),
                CompletedOrders = orderCountByState.GetValueOrDefault(OrderStatusEnum.Completed, 0),
                RevenueOverTime = new StatisticsChartDTO
                {
                    Labels = labels,
                    Datasets =
                    [
                        new StatisticsChartDatasetDTO
                        {
                            Label = "revenue",
                            Data = DashboardStatisticsUtil.BuildDoubleSeries(fromDate, toDate, revenueByDate)
                        }
                    ]
                },
                OrdersOverTime = new StatisticsChartDTO
                {
                    Labels = labels,
                    Datasets =
                    [
                        new StatisticsChartDatasetDTO
                        {
                            Label = "orders",
                            Data = DashboardStatisticsUtil.BuildDoubleSeries(fromDate, toDate, orderCountByDate)
                        }
                    ]
                },
                OrderStatus = new StatisticsDistributionDTO
                {
                    Labels = dashboardStates.Select(item => item.Key).ToList(),
                    Data = dashboardStates
                        .Select(item => orderCountByState.GetValueOrDefault(item.State, 0))
                        .ToList()
                }
            };

            return _mapper.Map<GetOrderStatisticsByDateRangeContract>(statistics);
        }

        public async Task<GetTopSellingMedicinesByDateRangeContract> GetTopSellingMedicinesByDateRangeAsync(DateOnly fromDate, DateOnly toDate, int topN)
        {
            OrderStatusEnum[] soldStates =
            [
                OrderStatusEnum.Paid,
                OrderStatusEnum.Shipping,
                OrderStatusEnum.Completed
            ];

            var effectiveTopN = topN > 0 ? topN : 5;
            var fromDateUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toDateExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var skuTotals = await _unitOfWork.Repository<OrderDetail>()
                .GetAllQueryable(includes: [nameof(OrderDetail.Order)])
                .Where(detail => detail.Order != null
                                 && detail.Order.OrderDate >= fromDateUtc
                                 && detail.Order.OrderDate < toDateExclusive
                                 && soldStates.Contains(detail.Order.OrderStatus))
                .GroupBy(detail => detail.SKUCode)
                .Select(g => new { SKUCode = g.Key, QuantitySold = g.Sum(d => d.Quantity) })
                .OrderByDescending(x => x.QuantitySold)
                .Take(effectiveTopN)
                .ToListAsync();

            if (skuTotals.Count == 0)
            {
                return _mapper.Map<GetTopSellingMedicinesByDateRangeContract>(
                    new TopSellingMedicinesStatisticsDTO { TopN = effectiveTopN, Items = [] });
            }

            var skuSet = skuTotals.Select(x => x.SKUCode).ToHashSet();
            var detailSnapshots = await _unitOfWork.Repository<OrderDetail>()
                .GetAllQueryable()
                .Where(d => skuSet.Contains(d.SKUCode))
                .Select(d => new { d.SKUCode, d.MedicineSnapshot })
                .ToListAsync();

            var snapshotDict = detailSnapshots
                .Where(d => d.MedicineSnapshot != null)
                .GroupBy(d => d.SKUCode)
                .ToDictionary(g => g.Key, g => g.First().MedicineSnapshot!);

            var topItems = skuTotals
                .Select(x => new TopSellingMedicineItemDTO
                {
                    SKUCode = x.SKUCode,
                    MedicineName = snapshotDict.TryGetValue(x.SKUCode, out var ms) && !string.IsNullOrWhiteSpace(ms.MedicineName)
                        ? ms.MedicineName
                        : x.SKUCode,
                    QuantitySold = x.QuantitySold
                })
                .ToList();

            var statistics = new TopSellingMedicinesStatisticsDTO
            {
                TopN = effectiveTopN,
                Items = topItems
            };

            return _mapper.Map<GetTopSellingMedicinesByDateRangeContract>(statistics);
        }
    }
}
