using AutoMapper;
using BillingSvc.BLL.DTOs.RefundDTOs;
using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BillingSvc.BLL.DTOs.ChargeDTOs
{
    public class GetChargeDTO : MapFrom<Charge>
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public string? ChargeType { get; set; }

        public double TotalAmount { get; set; }

        public int InvoiceId { get; set; }

        public int MedicalServiceId { get; set; }

        public GetChargeSnapshotDTO? ChargeSnapshot { get; set; }

        public GetRefundDTO? Refund { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Charge, GetChargeDTO>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.GetTotalAmount()));
        }
    }
}