using AutoMapper;
using PayrollSvc.BLL.DTOs.AllowanceDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Implements
{
    public class AllowanceService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Allowance, CreateAllowanceDTO, UpdateAllowanceDTO, GetAllowanceDTO>(unitOfWork, mapper), IAllowanceService;
}