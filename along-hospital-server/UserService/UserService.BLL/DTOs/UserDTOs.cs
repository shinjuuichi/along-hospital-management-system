using SharedLibrary.Base.Mappers;
using UserSvc.DAL.Models;

namespace UserSvc.BLL.DTOs
{
    public class CreateUserDTO : MapTo<User>
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
    }

    public class UpdateUserDTO : MapTo<User>
    {
        public string? Name { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? Role { get; set; }
    }

    public class GetUserDTO : MapFrom<User>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
    }
}