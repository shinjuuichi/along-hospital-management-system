using AutoMapper;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Contracts.RecruitmentContracts;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Contracts.StatisticsContracts;
using MessageBroker.Contracts.SupplierContracts;
using MessageBroker.Contracts.UserContracts;
using ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.DTOs.DashboardSharedDTOs;
using ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            MappingSharedDashboard();
            MappingManagerDashboard();
            MappingAccountantDashboard();
            MappingInventoryClerkDashboard();
            MappingPharmacistDashboard();
            MappingHrDashboard();
        }

        private void MappingSharedDashboard()
        {
            CreateMap<StatisticsChartDatasetContract, DashboardChartDatasetDTO>();
            CreateMap<StatisticsChartContract, DashboardChartDTO>();
            CreateMap<StatisticsDistributionContract, DashboardDistributionDTO>();
        }

        private void MappingManagerDashboard()
        {
            CreateMap<GetOrderStatisticsByDateRangeContract, ManagerOrderStatisticsDTO>();
            CreateMap<GetMedicalHistoryStatisticsByDateRangeContract, ManagerMedicalHistoryStatisticsDTO>();
            CreateMap<GetAppointmentStatisticsByDateRangeContract, ManagerAppointmentStatisticsDTO>();
            CreateMap<GetUserGrowthStatisticsByDateRangeContract, ManagerUserGrowthStatisticsDTO>();

            CreateMap<ManagerDashboardAggregateDTO, ManagerDashboardOverviewDTO>()
                .ForMember(dest => dest.OrderRevenue, opt => opt.MapFrom(src => src.Order == null ? 0 : src.Order.Revenue))
                .ForMember(dest => dest.NewPatients, opt => opt.MapFrom(src => src.UserGrowth == null ? 0 : src.UserGrowth.NewPatients))
                .ForMember(dest => dest.NewStaff, opt => opt.MapFrom(src => src.UserGrowth == null ? 0 : src.UserGrowth.NewStaff));

            CreateMap<ManagerDashboardAggregateDTO, ManagerDashboardStatisticsDTO>()
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.MedicalHistory))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment))
                .ForMember(dest => dest.UserGrowth, opt => opt.MapFrom(src => src.UserGrowth));
        }

        private void MappingAccountantDashboard()
        {
            CreateMap<GetBillingStatisticsByDateRangeContract, AccountantInvoiceStatisticsDTO>();
            CreateMap<GetBillingStatisticsByDateRangeContract, AccountantCollectionStatisticsDTO>();
            CreateMap<GetBillingStatisticsByDateRangeContract, AccountantRefundStatisticsDTO>();

            CreateMap<AccountantDashboardAggregateDTO, AccountantDashboardOverviewDTO>()
                .ForMember(dest => dest.TotalInvoices, opt => opt.MapFrom(src => src.Invoice == null ? 0 : src.Invoice.TotalInvoices))
                .ForMember(dest => dest.PendingInvoices, opt => opt.MapFrom(src => src.Invoice == null ? 0 : src.Invoice.PendingInvoices))
                .ForMember(dest => dest.CompletedInvoices, opt => opt.MapFrom(src => src.Invoice == null ? 0 : src.Invoice.CompletedInvoices))
                .ForMember(dest => dest.NetCollectedAmount, opt => opt.MapFrom(src => src.Collection == null ? 0 : src.Collection.NetCollectedAmount))
                .ForMember(dest => dest.RefundAmount, opt => opt.MapFrom(src => src.Refund == null ? 0 : src.Refund.RefundAmount))
                .ForMember(dest => dest.PendingRefunds, opt => opt.MapFrom(src => src.Refund == null ? 0 : src.Refund.PendingRefunds));

            CreateMap<AccountantDashboardAggregateDTO, AccountantDashboardStatisticsDTO>()
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => src.Invoice))
                .ForMember(dest => dest.Collection, opt => opt.MapFrom(src => src.Collection))
                .ForMember(dest => dest.Refund, opt => opt.MapFrom(src => src.Refund));
        }

        private void MappingInventoryClerkDashboard()
        {
            CreateMap<GetInventoryStatisticsByDateRangeContract, InventoryClerkInventoryStatisticsDTO>();
            CreateMap<GetSupplierStatisticsByDateRangeContract, InventoryClerkSupplierStatisticsDTO>();
            CreateMap<GetImportStatisticsByDateRangeContract, InventoryClerkImportStatisticsDTO>();

            CreateMap<InventoryClerkDashboardAggregateDTO, InventoryClerkDashboardOverviewDTO>()
                .ForMember(dest => dest.TotalInventoryItems, opt => opt.MapFrom(src => src.Inventory == null ? 0 : src.Inventory.TotalInventoryItems))
                .ForMember(dest => dest.LowStockItems, opt => opt.MapFrom(src => src.Inventory == null ? 0 : src.Inventory.LowStockItems))
                .ForMember(dest => dest.OutOfStockItems, opt => opt.MapFrom(src => src.Inventory == null ? 0 : src.Inventory.OutOfStockItems))
                .ForMember(dest => dest.TotalSuppliers, opt => opt.MapFrom(src => src.Supplier == null ? 0 : src.Supplier.TotalSuppliers))
                .ForMember(dest => dest.TotalImports, opt => opt.MapFrom(src => src.Import == null ? 0 : src.Import.TotalImports))
                .ForMember(dest => dest.TotalImportValue, opt => opt.MapFrom(src => src.Import == null ? 0 : src.Import.TotalImportValue));

            CreateMap<InventoryClerkDashboardAggregateDTO, InventoryClerkDashboardStatisticsDTO>()
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory))
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.Import, opt => opt.MapFrom(src => src.Import));
        }

        private void MappingPharmacistDashboard()
        {
            CreateMap<GetOrderStatisticsByDateRangeContract, PharmacistOrderStatisticsDTO>()
                .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Orders));
            CreateMap<GetTopSellingMedicinesByDateRangeContract, PharmacistTopMedicineStatisticsDTO>();
            CreateMap<GetTopSellingMedicineItemContract, PharmacistTopMedicineItemDTO>();

            CreateMap<PharmacistDashboardAggregateDTO, PharmacistDashboardOverviewDTO>()
                .ForMember(dest => dest.TotalOrders, opt => opt.MapFrom(src => src.Order == null ? 0 : src.Order.TotalOrders))
                .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.Order == null ? 0 : src.Order.Revenue))
                .ForMember(dest => dest.PendingOrders, opt => opt.MapFrom(src => src.Order == null ? 0 : src.Order.PendingOrders))
                .ForMember(dest => dest.CompletedOrders, opt => opt.MapFrom(src => src.Order == null ? 0 : src.Order.CompletedOrders));

            CreateMap<PharmacistDashboardAggregateDTO, PharmacistDashboardStatisticsDTO>()
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.TopMedicines, opt => opt.MapFrom(src => src.TopMedicines));
        }

        private void MappingHrDashboard()
        {
            CreateMap<GetRecruitmentStatisticsByDateRangeContract, HrRecruitmentStatisticsDTO>();
            CreateMap<GetStaffRequestStatisticsByDateRangeContract, HrLeaveStatisticsDTO>();

            CreateMap<HrDashboardAggregateDTO, HrDashboardOverviewDTO>()
                .ForMember(dest => dest.OpenJobPostings, opt => opt.MapFrom(src => src.Recruitment == null ? 0 : src.Recruitment.OpenJobPostings))
                .ForMember(dest => dest.ApplicationsReceived, opt => opt.MapFrom(src => src.Recruitment == null ? 0 : src.Recruitment.ApplicationsReceived))
                .ForMember(dest => dest.PendingLeaveRequests, opt => opt.MapFrom(src => src.Leave == null ? 0 : src.Leave.PendingLeaveRequests))
                .ForMember(dest => dest.ApprovedLeaveRequests, opt => opt.MapFrom(src => src.Leave == null ? 0 : src.Leave.ApprovedLeaveRequests));

            CreateMap<HrDashboardAggregateDTO, HrDashboardStatisticsDTO>()
                .ForMember(dest => dest.Overview, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Recruitment, opt => opt.MapFrom(src => src.Recruitment))
                .ForMember(dest => dest.Leave, opt => opt.MapFrom(src => src.Leave));
        }
    }
}
