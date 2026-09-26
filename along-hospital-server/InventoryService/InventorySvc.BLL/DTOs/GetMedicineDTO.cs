namespace InventorySvc.BLL.DTOs
{
    public class GetMedicineDTO
    {
        public int Id { get; set; }
        public string? SKUCode { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? MedicineUnit { get; set; }
        public string[] Images { get; set; } = [];
        public string? CategoryName { get; set; }
    }
}