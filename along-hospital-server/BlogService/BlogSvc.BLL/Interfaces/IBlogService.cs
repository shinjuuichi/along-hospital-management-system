using BlogSvc.BLL.DTOs.BlogDTOs;
using SharedLibrary.Base.Services;

namespace BlogSvc.BLL.Interfaces
{
    public interface IBlogService : IBaseCrudService<UpsertBlogDTO, UpsertBlogDTO, GetBlogDTO>;
}