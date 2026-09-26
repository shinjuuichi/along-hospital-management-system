namespace AuthSvc.BLL.DTOs.Response
{
    public class GetUserProfileDTO
    {
        public int AuthId { get; set; }

        public int UserId { get; set; }

        public string? Role { get; set; }

        public string? Stage { get; set; }
    }
}
