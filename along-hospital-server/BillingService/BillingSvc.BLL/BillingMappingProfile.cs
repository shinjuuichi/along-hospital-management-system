using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.BLL.DTOs.RefundDTOs;
using BillingSvc.BLL.DTOs.StatisticsDTOs;
using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.BillingEvents;
using MessageBroker.Events.PaymentEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace BillingSvc.BLL
{
    public class BillingMappingProfile : BaseMappingProfile
    {
        public BillingMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<BillingStatisticsDTO, GetBillingStatisticsByDateRangeContract>();

            // Event, DTO
            CreateMap<PaymentStatusChangedEvent, PaymentStatusChangedDTO>();
            CreateMap<CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent, CreateInvoiceDTO>();
            CreateMap<
                CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent
                    .CreateBedChargeInvoiceChargeItemEvent,
                CreateChargeDTO>();
            CreateMap<Charge, PaymentEventItem>()
                .ForMember(
                    d => d.ServiceName,
                    opt => opt.MapFrom(s =>
                        s.ChargeSnapshot != null && s.ChargeSnapshot.MedicalServiceName != null
                            ? s.ChargeSnapshot.MedicalServiceName
                            : string.Empty
                    )
                );

            // Contract, DTO
            CreateMap<GetStaffDataByUserIdContract, GetRefundStaffDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId))
                .ReverseMap();
            CreateMap<GetMedicalServiceContract, CreateChargeSnapshotDTO>()
                .ForMember(d => d.MedicalServiceName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.MedicalServiceCode, opt => opt.MapFrom(s => s.Code))
                .ForMember(d => d.MedicalServiceDescription, opt => opt.MapFrom(s => s.Description));
            CreateMap<CreateRefundContract, CreateChargeDTO>()
                .ForMember(d => d.ChargeType, opt => opt.MapFrom(s => nameof(ChargeTypeEnum.Refund)))
                .ForMember(d => d.Refund, opt => opt.MapFrom(s => new CreateRefundDTO
                {
                    Reason = s.Reason,
                    ClinicalMedicalOrderDetailId = s.ClinicalMedicalOrderDetailId
                }));

            MapContractItem();
        }

        private void MapContractItem()
        {
            CreateMap<GetInvoiceDTO,
                GetInvoiceContract>();

            CreateMap<GetChargeDTO,
                GetInvoiceContract
                    .GetChargeContract>();

            CreateMap<GetChargeSnapshotDTO,
                GetInvoiceContract.GetChargeContract.GetChargeSnapshotContract>();

            CreateMap<GetRefundDTO,
                GetInvoiceContract.GetChargeContract.GetRefundContract>();
        }
    }
}
