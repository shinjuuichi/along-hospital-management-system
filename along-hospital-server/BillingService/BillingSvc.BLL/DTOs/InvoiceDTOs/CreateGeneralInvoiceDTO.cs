using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BillingSvc.BLL.DTOs.InvoiceDTOs
{
    public class CreateGeneralInvoiceDTO : MapTo<Invoice>
    {
        public int MedicalHistoryId { get; set; }

        public string? MedicalHistoryType { get; set; }

        public bool IsPaid { get; set; } = false;

        public List<CreateChargeDTO> Charges { get; set; } = [];

        public string? InvoiceNumber { get; set; }
    }
}
