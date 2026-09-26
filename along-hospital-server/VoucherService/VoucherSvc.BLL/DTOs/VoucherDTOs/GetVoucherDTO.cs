using AutoMapper;
using SharedLibrary.Base.Mappers;
using VoucherSvc.BLL.DTOs.PatientVoucherDTOs;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.DTOs.VoucherDTOs
{
    public class GetVoucherDTO : MapFrom<Voucher>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int Quantity { get; set; }
        public string? VoucherStatus { get; set; }
        public DateOnly ExpireDate { get; set; }
        public string? DiscountType { get; set; }
        public double DiscountValue { get; set; }
        public double? MaxDiscount { get; set; }
        public double? MinPurchaseAmount { get; set; }
        public string? VoucherType { get; set; }
        public string? Image { get; set; }

        public List<int> MedicineIds { get; set; } = [];
        public List<MedicineInfo> Medicines { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            base.Mapping(profile);

            profile.CreateMap<MedicineDiscount, GetVoucherDTO>()
                .IncludeBase<Voucher, GetVoucherDTO>();
        }
    }

    public class MedicineInfo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public double Price { get; set; }
        public string? CategoryName { get; set; }
    }
}
