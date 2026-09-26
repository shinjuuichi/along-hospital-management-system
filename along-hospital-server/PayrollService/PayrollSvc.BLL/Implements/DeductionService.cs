using AutoMapper;
using PayrollSvc.BLL.DTOs.DeductionDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Implements
{
    public class DeductionService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Deduction, CreateDeductionDTO, UpdateDeductionDTO, GetDeductionDTO>(unitOfWork, mapper), IDeductionService;
}