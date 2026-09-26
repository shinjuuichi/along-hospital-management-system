using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BillingSvc.BLL.DTOs.RefundDTOs
{
    public class GetRefundDTO : MapFrom<Refund>
    {
        public int Id { get; set; }

        public string? Reason { get; set; }

        public string? RefundStatus { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public int? ApprovedBy { get; set; }

        public int ChargeId { get; set; }

        public string? ClinicalMedicalOrderDetailId { get; set; }

        public GetRefundStaffDTO? Staff { get; set; }
    }
}