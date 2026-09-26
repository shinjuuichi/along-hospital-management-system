using AutoMapper;
using BillingSvc.BLL.DTOs.StatisticsDTOs;
using BillingSvc.BLL.Interfaces;
using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using MessageBroker.Contracts.BillingContracts;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;

namespace BillingSvc.BLL.Implements
{
    public class BillingStatisticsService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IBillingStatisticsService
    {
        private readonly IGenericRepository<Invoice> _invoiceRepository = unitOfWork.Repository<Invoice>();
        private readonly IGenericRepository<Refund> _refundRepository = unitOfWork.Repository<Refund>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetBillingStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var invoicesInRange = await _invoiceRepository.GetAllAsync(
                invoice =>
                    invoice.CreationDate >= fromUtc &&
                    invoice.CreationDate < toExclusive,
                [nameof(Invoice.Charges)]);

            var collectedInvoices = await _invoiceRepository.GetAllAsync(
                invoice =>
                    invoice.InvoiceStatus == InvoiceStatusEnum.Completed &&
                    invoice.PaymentDate.HasValue &&
                    invoice.PaymentDate.Value >= fromUtc &&
                    invoice.PaymentDate.Value < toExclusive,
                [nameof(Invoice.Charges)]);

            var refundsInRange = await _refundRepository.GetAllAsync(
                refund =>
                    refund.CreationDate >= fromUtc &&
                    refund.CreationDate < toExclusive,
                [nameof(Refund.Charge)]);

            var approvedRefunds = await _refundRepository.GetAllAsync(
                refund =>
                    refund.RefundStatus == RefundStatusEnum.Approved &&
                    refund.ApprovalDate.HasValue &&
                    refund.ApprovalDate.Value >= fromUtc &&
                    refund.ApprovalDate.Value < toExclusive,
                [nameof(Refund.Charge)]);

            var invoiceStatusCounts = invoicesInRange
                .GroupBy(invoice => invoice.InvoiceStatus)
                .ToDictionary(group => group.Key, group => group.Count());
            var refundStatusCounts = refundsInRange
                .GroupBy(refund => refund.RefundStatus)
                .ToDictionary(group => group.Key, group => group.Count());

            var invoiceCountsByDay = invoicesInRange
                .GroupBy(invoice => DateOnly.FromDateTime(invoice.CreationDate))
                .ToDictionary(group => group.Key, group => group.Count());
            var collectionAmountsByDay = collectedInvoices
                .GroupBy(invoice => DateOnly.FromDateTime(invoice.PaymentDate!.Value))
                .ToDictionary(group => group.Key, group => group.Sum(invoice => invoice.GetTotalInvoiceAmount()));
            var refundAmountsByDay = approvedRefunds
                .GroupBy(refund => DateOnly.FromDateTime(refund.ApprovalDate!.Value))
                .ToDictionary(group => group.Key, group => group.Sum(refund => refund.Charge?.GetTotalAmount() ?? 0d));

            var grossInvoiceAmount = invoicesInRange.Sum(invoice => invoice.GetTotalInvoiceAmount());
            var collectedAmount = collectedInvoices.Sum(invoice => invoice.GetTotalInvoiceAmount());
            var refundAmount = approvedRefunds.Sum(refund => refund.Charge?.GetTotalAmount() ?? 0d);

            var statistics = new BillingStatisticsDTO
            {
                TotalInvoices = invoicesInRange.Count,
                CompletedInvoices = invoicesInRange.Count(invoice => invoice.InvoiceStatus == InvoiceStatusEnum.Completed),
                PendingInvoices = invoicesInRange.Count(invoice => invoice.InvoiceStatus == InvoiceStatusEnum.Pending),
                CancelledInvoices = invoicesInRange.Count(invoice => invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled),
                GrossInvoiceAmount = grossInvoiceAmount,
                CollectedAmount = collectedAmount,
                RefundAmount = refundAmount,
                NetCollectedAmount = collectedAmount - refundAmount,
                PendingRefunds = refundsInRange.Count(refund => refund.RefundStatus == RefundStatusEnum.Pending),
                ApprovedRefunds = refundsInRange.Count(refund => refund.RefundStatus == RefundStatusEnum.Approved),
                CancelledRefunds = refundsInRange.Count(refund => refund.RefundStatus == RefundStatusEnum.Cancelled),
                InvoiceStatus = StatisticsContractBuilder.CreateDistribution(invoiceStatusCounts),
                RefundStatus = StatisticsContractBuilder.CreateDistribution(refundStatusCounts),
                InvoicesOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(fromDate, toDate, "invoices", invoiceCountsByDay),
                CollectionsOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(fromDate, toDate, "collections", collectionAmountsByDay),
                RefundsOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(fromDate, toDate, "refunds", refundAmountsByDay)
            };

            return _mapper.Map<GetBillingStatisticsByDateRangeContract>(statistics);
        }
    }
}
