namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class UpdateMedicineAndInventoryDTO : UpdateMedicineDTO
    {
        public int MedicineId { get; set; }

        public int Quantity { get; set; }

        public string? Status { get; set; }

        public DateTime? LastImportDate { get; set; }

        public int? MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }
    }
}
