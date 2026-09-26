using AuthSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AuthSvc.BLL.DTOs
{
    public class GetAuthDTO : MapFrom<AuthAccount>
    {
        public int UserId { get; init; }
        public string? Phone { get; init; }
        public string? Email { get; init; }
    }
}
