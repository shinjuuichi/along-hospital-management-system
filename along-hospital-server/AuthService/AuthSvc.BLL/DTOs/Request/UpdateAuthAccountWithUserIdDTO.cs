namespace AuthSvc.BLL.DTOs.Request
{
    public class UpdateAuthAccountWithUserIdDTO
    {
        public int AuthId { get; set; }
        public int UserId { get; set; }
        public string? Phone { get; set; }
    }
}
