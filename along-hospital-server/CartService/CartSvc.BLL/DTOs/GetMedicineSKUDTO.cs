using MessageBroker.Contracts.MedicineContracts;

namespace CartSvc.BLL.DTOs
{
    public class GetMedicineSKUDTO
    {
        public int Id { get; set; }

        public string? SKUCode { get; set; }

        public double Price { get; set; }

        public bool IsActive { get; set; }

        public int MedicineId { get; set; }

        public string? MedicineName { get; set; }

        public string? MedicineBrand { get; set; }

        public string? MedicineUnit { get; set; }

        public string[] MedicineImages { get; set; } = [];

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public bool IsPublic { get; set; }

        public List<SKUValueContractItem> SKUValues { get; set; } = [];

        public int Quantity { get; set; }
    }
}
