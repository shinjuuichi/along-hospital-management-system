using Microsoft.AspNetCore.Http;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.DTOs.VoucherDTOs
{
    public class UpdateVoucherDTO : MapTo<Voucher>, IUploadImageDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? VoucherStatus { get; set; }
        public DateOnly ExpireDate { get; set; }
        public int? Quantity { get; set; }
        public string? DiscountType { get; set; }
        public double? DiscountValue { get; set; }
        public double? MaxDiscount { get; set; }
        public double? MinPurchaseAmount { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }

        public List<int> MedicineIds { get; set; } = [];
    }
}
