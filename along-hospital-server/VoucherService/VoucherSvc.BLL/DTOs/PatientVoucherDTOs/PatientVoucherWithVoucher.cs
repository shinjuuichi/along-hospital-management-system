using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.DTOs.PatientVoucherDTOs
{
    public class PatientVoucherWithVoucher
    {
        public PatientVoucher PatientVoucher { get; set; } = null!;
        public Voucher Voucher { get; set; } = null!;
    }
}
