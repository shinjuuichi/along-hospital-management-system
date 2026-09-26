using AutoMapper;
using BlogSvc.BLL.DTOs.BlogDTOs;
using BlogSvc.BLL.Interfaces;
using BlogSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Services.Interfaces;

namespace BlogSvc.BLL.Implements
{
    public class BlogService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IUploadFileService _uploadFileService)
        : BaseService<Blog, UpsertBlogDTO, UpsertBlogDTO, GetBlogDTO>(_unitOfWork, _mapper, _uploadFileService, includes: ["BlogCategory"]),
            IBlogService;
}
