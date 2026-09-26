namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs
{
    public class GetMedicalHistoryClinicalMedicalOrderDTO : GetMedicalHistoryMedicalOrderDTO
    {
        public string? ClinicalMedicalOrderStatus { get; set; }

        public int PendingInvoiceId { get; set; }

        public List<ClinicalMedicalOrderDetailDTO> ClinicalMedicalOrderDetails { get; set; } = [];

        public class ClinicalMedicalOrderDetailDTO
        {
            public string? Id { get; set; }

            public int Quantity { get; set; }

            public string? ClinicalMedicalOrderDetailStatus { get; set; }

            public int MedicalServiceId { get; set; }

            public MedicalServiceSnapshotDTO? MedicalServiceSnapshot { get; set; }

            public class MedicalServiceSnapshotDTO
            {
                public string? Name { get; set; }

                public string? Description { get; set; }

                public string? Code { get; set; }
            }
        }
    }
}
