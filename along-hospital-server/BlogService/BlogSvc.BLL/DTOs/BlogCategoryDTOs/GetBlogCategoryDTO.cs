using BlogSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BlogSvc.BLL.DTOs.BlogCategoryDTOs
{
    public class GetBlogCategoryDTO : MapFrom<BlogCategory>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}