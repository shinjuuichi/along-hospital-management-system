using InpatientResourceSvc.BLL.DTOs.RoomDTOs;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IRoomService : IBaseCrudService<CreateRoomDTO, UpdateRoomDTO, GetRoomDTO>
    {
        Task<List<GetRoomDTO>> GetAllByRolesAsync();
    }
}