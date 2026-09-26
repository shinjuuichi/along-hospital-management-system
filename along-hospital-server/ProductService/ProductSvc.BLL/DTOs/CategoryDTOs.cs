using Microsoft.AspNetCore.Http;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;

namespace ProductSvc.BLL.DTOs
{
    public class CreateCategoryDTO : MapTo<Category>, IUploadImageDTO
    {
        public string Name { get; set; } = null!;

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }
    }

    public class UpdateCategoryDTO : MapTo<Category>, IUploadImageDTO
    {
        public string Name { get; set; } = null!;

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }
    }

    public class GetCategoryDTO : MapFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}
