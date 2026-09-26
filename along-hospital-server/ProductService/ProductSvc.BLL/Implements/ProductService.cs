using AutoMapper;
using ProductSvc.BLL.DTOs;
using ProductSvc.BLL.Interfaces;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Services.Interfaces;

namespace ProductSvc.BLL.Implements
{
    public class ProductService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IUploadFileService _uploadFileService)
        : BaseService<Product, CreateProductDTO, UpdateProductDTO, GetProductDTO>(_unitOfWork, _mapper, _uploadFileService, ["Category", "ProductDetail"]), IProductService
    {
        private IGenericRepository<ProductDetail> _productDetailRepository => _unitOfWork.Repository<ProductDetail>();

        public override async Task<GetProductDTO> UpdateAsync(int id, UpdateProductDTO dto)
        {
            var productDetailDto = dto.ProductDetail;
            dto.ProductDetail = null;

            var product = await base.UpdateAsync(id, dto);

            if (productDetailDto != null)
            {
                var existingDetail = await _productDetailRepository.GetByConditionAsync(d => d.ProductId == id);

                if (existingDetail != null)
                {
                    _mapper.Map(productDetailDto, existingDetail);
                    _productDetailRepository.Update(existingDetail);
                }
                else
                {
                    var newDetail = _mapper.Map<ProductDetail>(productDetailDto);
                    newDetail.ProductId = id;
                    await _productDetailRepository.AddAsync(newDetail);
                }

                await _unitOfWork.SaveChangeAsync();
            }

            return product;
        }
    }
}

