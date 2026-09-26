using SharedLibrary.Base.Services;
using WorkScheduleSvc.BLL.DTOs.HolidayDTOs;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IHolidayService : IBaseCrudService<UpsertHolidayDTO, UpsertHolidayDTO, GetHolidayDTO>
    {
        Task<List<GetHolidayDTO>> CreateRangeAsync(CreateHolidayRangeDTO dto);
    }
}