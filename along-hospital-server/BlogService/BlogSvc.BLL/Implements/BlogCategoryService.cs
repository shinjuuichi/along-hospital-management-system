using AutoMapper;
using BlogSvc.BLL.DTOs.BlogCategoryDTOs;
using BlogSvc.BLL.Interfaces;
using BlogSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace BlogSvc.BLL.Implements
{
    public class BlogCategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<BlogCategory, UpsertBlogCategoryDTO, UpsertBlogCategoryDTO, GetBlogCategoryDTO>(unitOfWork, mapper),
            IBlogCategoryService;
}