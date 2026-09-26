namespace EmailSvc.DTOs
{
    public class SendLinkEmailDTO
    {
        public string Email { get; set; } = default!;

        public string Subject { get; set; } = default!;

        public string Link { get; set; } = default!;
    }
}
