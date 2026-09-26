using InpatientResourceSvc.BLL.DTOs.BedCategoryDTOs;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IBedCategoryService
        : IBaseCrudService<CreateBedCategoryDTO, UpdateBedCategoryDTO, GetBedCategoryDTO>;
}