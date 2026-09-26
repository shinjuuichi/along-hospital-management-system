using BlogSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BlogSvc.BLL.DTOs.BlogCategoryDTOs
{
    public class UpsertBlogCategoryDTO : MapTo<BlogCategory>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
