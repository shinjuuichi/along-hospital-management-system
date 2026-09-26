using Microsoft.AspNetCore.Http;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;
using UserSvc.DAL.Models;

namespace UserSvc.BLL.DTOs
{
    public class UpdateProfileUserDTO : MapTo<User>, IUploadImageDTO
    {
        public string? Name { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }
    }
}