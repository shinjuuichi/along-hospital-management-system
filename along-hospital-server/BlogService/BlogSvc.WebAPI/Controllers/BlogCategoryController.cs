using BlogSvc.BLL.DTOs.BlogCategoryDTOs;
using BlogSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace BlogSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Marketer))]
    public class BlogCategoryController(IBlogCategoryService blogCategoryService)
        : CrudController<UpsertBlogCategoryDTO, UpsertBlogCategoryDTO, GetBlogCategoryDTO>(blogCategoryService)
    {
        protected override string? EntityName => "Blog Category";

        [AllowAnonymous]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }
    }
}