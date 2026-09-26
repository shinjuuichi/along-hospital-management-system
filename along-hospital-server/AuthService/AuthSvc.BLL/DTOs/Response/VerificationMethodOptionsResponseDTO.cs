namespace AuthSvc.BLL.DTOs.Response
{
    public class VerificationMethodOptionsResponseDTO
    {
        public string Identifier { get; set; } = string.Empty;

        public string DefaultDeliveryMethod { get; set; } = string.Empty;

        public List<VerificationMethodOptionDTO> Methods { get; set; } = [];
    }
}
