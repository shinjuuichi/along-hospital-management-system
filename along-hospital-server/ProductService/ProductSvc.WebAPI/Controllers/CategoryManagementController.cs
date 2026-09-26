using ProductSvc.BLL.DTOs;
using ProductSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace ProductSvc.WebAPI.Controllers
{
    public class CategoryManagementController(ICategoryService _categoryService)
        : CrudController<CreateCategoryDTO, UpdateCategoryDTO, GetCategoryDTO>(_categoryService)
    {
        protected override string? EntityName => "Category";
    }
}