using MedicineSvc.BLL.DTOs.MedicineCategoryDTOs;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Interfaces
{
    public interface IMedicineCategoryService
        : IBaseCrudService<UpsertMedicineCategoryDTO, UpsertMedicineCategoryDTO, GetMedicineCategoryDTO>;
}
