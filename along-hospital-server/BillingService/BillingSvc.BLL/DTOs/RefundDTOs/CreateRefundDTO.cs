using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BillingSvc.BLL.DTOs.RefundDTOs
{
    public class CreateRefundDTO : MapTo<Refund>
    {
        public string? Reason { get; set; }

        public string? ClinicalMedicalOrderDetailId { get; set; }
    }
}