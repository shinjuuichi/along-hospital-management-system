namespace AuthSvc.BLL.DTOs.Response
{
    public class VerificationMethodOptionDTO
    {
        public string DeliveryMethod { get; set; } = string.Empty;

        public string MaskedDestination { get; set; } = string.Empty;
    }
}
