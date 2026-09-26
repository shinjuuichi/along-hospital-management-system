using InpatientResourceSvc.BLL.DTOs.RoomCategoryDTOs;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IRoomCategoryService
        : IBaseCrudService<UpsertRoomCategoryDTO, UpsertRoomCategoryDTO, GetRoomCategoryDTO>;
}
