using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Events.PaymentEvents;
using PaymentSvc.BLL.DTOs.CashDTOs;
using PaymentSvc.BLL.DTOs.PayOSDTOs;
using PaymentSvc.BLL.DTOs.SePayDTOs;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace PaymentSvc.BLL
{
    public class PaymentMappingProfile : BaseMappingProfile
    {
        public PaymentMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MappingPayOS();
            MappingCash();
            MappingSePay();
            MappingPayment();
        }

        private void MappingPayOS()
        {
            CreateMap<CreatePaymentEvent, CreatePayOSDTO>()
                .ForMember(dest => dest.PayOsItemDTOs, opt => opt.MapFrom(src => src.PaymentEventItems));

            CreateMap<PaymentEventItem, PayOsItemDTO>();

            CreateMap<PayOsItemDTO, PaymentItem>();

            CreateMap<CreatePayOSDTO, PayOSPayment>()
                .ForMember(dest => dest.PaymentItems, opt => opt.MapFrom(src => src.PayOsItemDTOs))
                .ForMember(dest => dest.OriginalAmount, opt => opt.Ignore());
        }

        private void MappingCash()
        {
            CreateMap<CreatePaymentEvent, CreateCashDTO>()
                .ForMember(dest => dest.CashItemDTOs, opt => opt.MapFrom(src => src.PaymentEventItems));

            CreateMap<PaymentEventItem, CashItemDTO>();

            CreateMap<CashItemDTO, PaymentItem>();

            CreateMap<CreateCashDTO, CashPayment>()
                .ForMember(dest => dest.PaymentItems, opt => opt.MapFrom(src => src.CashItemDTOs))
                .ForMember(dest => dest.OriginalAmount, opt => opt.Ignore());
        }

        private void MappingSePay()
        {
            CreateMap<CreatePaymentEvent, CreateSePayDTO>()
              .ForMember(dest => dest.SePayItemDTOs, opt => opt.MapFrom(src => src.PaymentEventItems));

            CreateMap<CreatePaymentEventForPayroll, CreateSePayDTO>()
              .ForMember(dest => dest.SePayItemDTOs, opt => opt.MapFrom(src => src.PaymentEventItems));

            CreateMap<PaymentEventItem, SePayItemDTO>();

            CreateMap<SePayItemDTO, PaymentItem>();

            CreateMap<CreateSePayDTO, SePayPayment>()
              .ForMember(dest => dest.PaymentItems, opt => opt.MapFrom(src => src.SePayItemDTOs))
              .ForMember(dest => dest.OriginalAmount, opt => opt.Ignore());
        }

        private void MappingPayment()
        {
            CreateMap<Payment, PaymentStatusChangedEvent>()
             .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<GetSePayDTO, CreatePaymentForPayrollContract>();
        }
    }
}