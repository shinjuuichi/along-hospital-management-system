using AutoMapper;
using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BillingSvc.BLL.DTOs.InvoiceDTOs
{
    public class GetInvoiceDTO : MapFrom<Invoice>
    {
        public int Id { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? InvoiceStatus { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double TotalInvoiceAmount { get; set; }

        public double TotalRefundAmount { get; set; }

        public double TotalAmount { get; set; }

        public int MedicalHistoryId { get; set; }

        public string? ClinicalMedicalOrderId { get; set; }

        public List<GetChargeDTO> Charges { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Invoice, GetInvoiceDTO>()
                .ForMember(dest => dest.TotalInvoiceAmount, opt => opt.MapFrom(src => src.GetTotalInvoiceAmount()))
                .ForMember(dest => dest.TotalRefundAmount, opt => opt.MapFrom(src => src.GetTotalRefundAmount()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.GetTotalAmount()));
        }
    }
}