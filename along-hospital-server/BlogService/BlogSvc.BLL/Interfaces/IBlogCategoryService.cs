using BlogSvc.BLL.DTOs.BlogCategoryDTOs;
using SharedLibrary.Base.Services;

namespace BlogSvc.BLL.Interfaces
{
    public interface IBlogCategoryService
        : IBaseCrudService<UpsertBlogCategoryDTO, UpsertBlogCategoryDTO, GetBlogCategoryDTO>;
}