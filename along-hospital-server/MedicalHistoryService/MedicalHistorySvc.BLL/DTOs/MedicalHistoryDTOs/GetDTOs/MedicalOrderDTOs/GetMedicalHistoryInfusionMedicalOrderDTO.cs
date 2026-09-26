namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs
{
    public class GetMedicalHistoryInfusionMedicalOrderDTO : GetMedicalHistoryMedicalOrderDTO
    {
        public List<InfusionMedicalOrderDetailDTO> InfusionMedicalOrderDetails { get; set; } = [];

        public class InfusionMedicalOrderDetailDTO
        {
            public string? Id { get; set; }

            public string? Rate { get; set; }

            public string? Frequency { get; set; }

            public string? Duration { get; set; }

            public string? InfusionMedicalOrderDetailExecutionStatus { get; set; }

            public string? Note { get; set; }

            public int MedicineId { get; set; }

            public MedicineSnapshotDTO? MedicineSnapshot { get; set; }

            public class MedicineSnapshotDTO
            {
                public string? Name { get; set; }

                public string? Brand { get; set; }

                public string? MedicineUnit { get; set; }

                public string? MedicineImage { get; set; }

                public string? CategoryName { get; set; }
            }
        }
    }
}
