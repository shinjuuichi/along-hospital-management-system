using BlogSvc.BLL.DTOs.BlogCategoryDTOs;
using BlogSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace BlogSvc.BLL.DTOs.BlogDTOs
{
    public class GetBlogDTO : MapFrom<Blog>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Content { get; set; }
        public DateTime? CreationDate { get; set; }
        public GetBlogCategoryDTO? BlogCategory { get; set; }
    }
}