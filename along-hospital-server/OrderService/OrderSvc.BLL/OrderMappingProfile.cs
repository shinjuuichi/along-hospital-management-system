using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Contracts.StatisticsContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.OrderEvents;
using MessageBroker.Events.PaymentEvents;
using OrderSvc.BLL.DTOs;
using OrderSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace OrderSvc.BLL
{
    public class OrderMappingProfile : BaseMappingProfile
    {
        public OrderMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<StatisticsChartDatasetDTO, StatisticsChartDatasetContract>();
            CreateMap<StatisticsChartDTO, StatisticsChartContract>();
            CreateMap<StatisticsDistributionDTO, StatisticsDistributionContract>();
            CreateMap<OrderStatisticsDTO, GetOrderStatisticsByDateRangeContract>();
            CreateMap<TopSellingMedicinesStatisticsDTO, GetTopSellingMedicinesByDateRangeContract>();
            CreateMap<TopSellingMedicineItemDTO, GetTopSellingMedicineItemContract>();

            CreateMap<CartMedicineEventItem, CreateOrderDetailDTO>();

            CreateMap<CreateOrderDTO, CreatePaymentEvent>();

            CreateMap<CreateOrderByCartDataEvent, CreateOrderDTO>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.CartMedicineEventItems))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<CreateOrderDetailDTO, CheckQuantityOfSKUEventItem>();
            CreateMap<OrderDetail, DecreaseInventoryQuantityEventItem>();
            CreateMap<CreateOrderDetailDTO, DecreaseInventoryQuantityEventItem>();

            CreateMap<OrderDetail, AddQuantityFromImportEvent>();
        }
    }
}