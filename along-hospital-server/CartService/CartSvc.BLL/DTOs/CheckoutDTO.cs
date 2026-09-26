namespace CartSvc.BLL.DTOs
{
    public class CheckoutDTO
    {
        public string? VoucherCode { get; set; }
        public string? PaymentType { get; set; }
        public string? Description { get; set; }
        public List<string> SelectedSKUCodes { get; set; } = [];
        public bool IsPickupAtStore { get; set; }
    }
}