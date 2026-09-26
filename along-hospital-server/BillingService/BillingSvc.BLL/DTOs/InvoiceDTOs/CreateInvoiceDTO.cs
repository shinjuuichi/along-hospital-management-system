using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace BillingSvc.BLL.DTOs.InvoiceDTOs
{
    public class CreateInvoiceDTO : MapTo<Invoice>
    {
        public int MedicalHistoryId { get; set; }

        public List<CreateChargeDTO> Charges { get; set; } = [];

        public string? ClinicalMedicalOrderId { get; set; }

        [JsonIgnore]
        public string? InvoiceNumber { get; set; }
    }
}