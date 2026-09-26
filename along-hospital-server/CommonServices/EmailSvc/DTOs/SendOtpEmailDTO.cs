namespace EmailSvc.DTOs
{
    public class SendOtpEmailDTO
    {
        public string Email { get; set; } = default!;

        public string Subject { get; set; } = default!;

        public string Otp { get; set; } = default!;
    }
}
