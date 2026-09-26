namespace CartSvc.BLL.DTOs
{
    public class GetMedicineDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string[] Images { get; set; } = [];
        public string? MedicineUnit { get; set; }
        public int Price { get; set; }
    }
}
