using ProductSvc.BLL.DTOs;
using ProductSvc.BLL.FilterDTOs;
using ProductSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace ProductSvc.WebAPI.Controllers
{
    public class ProductManagementController(IProductService _productService)
        : CrudController<CreateProductDTO, UpdateProductDTO, GetProductDTO, ProductFilter>(_productService)
    {
        protected override string EntityName => "Product";
    }
}
