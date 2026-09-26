using AutoMapper;
using MedicineSvc.BLL.DTOs.MedicineCategoryDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Services.Interfaces;

namespace MedicineSvc.BLL.Implements
{
    public class MedicineCategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadFileService uploadFileService)
            : BaseService<MedicineCategory, UpsertMedicineCategoryDTO, UpsertMedicineCategoryDTO, GetMedicineCategoryDTO>(unitOfWork, mapper, uploadFileService),
                IMedicineCategoryService;
}
