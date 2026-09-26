using SharedLibrary.Base.Services;
using TeleHealthSvc.BLL.DTOs.TeleRoomDTOs;

namespace TeleHealthSvc.BLL.Interfaces
{
    public interface ITeleRoomService : IBaseCrudService<CreateTeleRoomDTO, UpdateTeleRoomDTO, GetTeleRoomDTO>
    {
        Task<GetTeleRoomWithCredentialsDTO> GetTeleRoomForDoctorAsync();
    }
}
