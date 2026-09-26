namespace AuthSvc.BLL.DTOs.Response
{
    public class GetAuthResponseDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? AccessTokenExpires { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
        public string? Role { get; set; }
        public int? AuthId { get; set; }
        public int? UserId { get; set; }
        public string? Stage { get; set; }
    }
}
