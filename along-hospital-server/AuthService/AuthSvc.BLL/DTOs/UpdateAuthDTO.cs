using AuthSvc.DAL.Models;
using AutoMapper;
using SharedLibrary.Base.Mappers;

namespace AuthSvc.BLL.DTOs
{
    public class UpdateAuthDTO : MapTo<AuthAccount>
    {
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public int UserId { get; set; }
    }
}
