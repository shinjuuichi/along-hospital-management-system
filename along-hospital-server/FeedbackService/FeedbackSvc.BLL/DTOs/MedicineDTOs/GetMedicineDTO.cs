namespace FeedbackSvc.BLL.DTOs.MedicineDTOs
{
    public class GetMedicineDTO
    {
        public int MedicineId { get; set; }

        public string? MedicineName { get; set; }

        public string? MedicineBrand { get; set; }

        public string[] MedicineImages { get; set; } = [];
    }
}