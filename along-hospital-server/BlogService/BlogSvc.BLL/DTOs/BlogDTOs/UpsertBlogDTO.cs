using BlogSvc.DAL.Models;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;

namespace BlogSvc.BLL.DTOs.BlogDTOs
{
    public class UpsertBlogDTO : MapTo<Blog>, IUploadImageDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int BlogCategoryId { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }
    }
}