using AutoMapper;
using ProductSvc.BLL.DTOs;
using ProductSvc.BLL.Interfaces;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Services.Interfaces;

namespace ProductSvc.BLL.Implements
{
    public class CategoryService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IUploadFileService _uploadFileService)
        : BaseService<Category, CreateCategoryDTO, UpdateCategoryDTO, GetCategoryDTO>(_unitOfWork, _mapper, _uploadFileService), ICategoryService
    {
    }
}
