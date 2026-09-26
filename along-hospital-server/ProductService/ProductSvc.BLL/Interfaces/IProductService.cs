using ProductSvc.BLL.DTOs;
using SharedLibrary.Base.Services;

namespace ProductSvc.BLL.Interfaces
{
    public interface IProductService : IBaseCrudService<CreateProductDTO, UpdateProductDTO, GetProductDTO>
    {
    }
}
