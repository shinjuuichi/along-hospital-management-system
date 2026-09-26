using BlogSvc.BLL.DTOs.BlogDTOs;
using BlogSvc.BLL.FilterDTOs;
using BlogSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace BlogSvc.WebAPI.Controllers
{
    public class BlogController(IBlogService _blogService)
        : GetController<GetBlogDTO, BlogFilterDTO>(_blogService);
}