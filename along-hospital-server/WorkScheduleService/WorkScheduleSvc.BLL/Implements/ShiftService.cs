using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using WorkScheduleSvc.BLL.DTOs.ShiftDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class ShiftService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Shift, CreateShiftDTO, UpdateShiftDTO, GetShiftDTO>(unitOfWork, mapper), IShiftService;
}
