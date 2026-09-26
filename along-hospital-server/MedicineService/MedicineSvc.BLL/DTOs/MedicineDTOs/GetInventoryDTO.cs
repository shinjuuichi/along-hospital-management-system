namespace MedicineSvc.BLL.DTOs.MedicineDTOs
{
    public class GetInventoryDTO
    {
        public int Id { get; set; }

        public string? SKUCode { get; set; }

        public int Quantity { get; set; }

        public int? MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }
    }
}
