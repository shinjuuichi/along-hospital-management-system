using BlogSvc.BLL.DTOs.BlogDTOs;
using BlogSvc.BLL.FilterDTOs;
using BlogSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace BlogSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Marketer))]
    public class BlogManagementController(IBlogService _blogService)
        : CrudController<UpsertBlogDTO, UpsertBlogDTO, GetBlogDTO, BlogFilterDTO>(_blogService)
    {
        protected override string? EntityName => "Blog";
    }
}