using AutoMapper;
using SharedLibrary.Base.Mappers;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.DTOs.PatientVoucherDTOs
{
    public class GetMyPatientVoucherDTO : MapFrom<Voucher>
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly ExpireDate { get; set; }
        public string? DiscountType { get; set; }
        public double DiscountValue { get; set; }
        public double? MaxDiscount { get; set; }
        public double? MinPurchaseAmount { get; set; }
        public string? Image { get; set; }
    }
}
