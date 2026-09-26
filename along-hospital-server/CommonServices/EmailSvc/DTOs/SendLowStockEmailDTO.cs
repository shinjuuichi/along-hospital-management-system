namespace EmailSvc.DTOs
{
    public class SendLowStockMedicineDTO
    {
        public int MedicineId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? MedicineUnit { get; set; }
        public string[] Images { get; set; } = [];
        public string? CategoryName { get; set; }
        public int Quantity { get; set; }
        public int? MinQuantity { get; set; }
        public DateTime? LastImportDate { get; set; }
    }

    public class SendLowStockMedicinesEmailDTO
    {
        public string Email { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public List<SendLowStockMedicineDTO> Medicines { get; set; } = [];
    }
}
