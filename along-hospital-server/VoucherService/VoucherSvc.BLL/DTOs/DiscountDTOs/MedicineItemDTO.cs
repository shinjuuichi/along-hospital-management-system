namespace VoucherSvc.BLL.DTOs.DiscountDTOs
{
    public class MedicineItemDTO
    {
        public int MedicineId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string? SKUCode { get; set; }
    }
}
