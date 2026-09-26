using ProductSvc.BLL.DTOs;
using SharedLibrary.Base.Services;

namespace ProductSvc.BLL.Interfaces
{
    public interface ICategoryService : IBaseCrudService<CreateCategoryDTO, UpdateCategoryDTO, GetCategoryDTO>
    {
    }
}
