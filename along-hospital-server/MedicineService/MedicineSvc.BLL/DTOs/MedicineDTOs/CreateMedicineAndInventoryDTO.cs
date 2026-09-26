namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class CreateMedicineAndInventoryDTO : CreateMedicineDTO
    {
        public int Quantity { get; set; }

        public int? MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }
    }
}
