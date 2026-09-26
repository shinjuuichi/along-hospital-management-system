using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.RegionalWageDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class RegionalWageService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<RegionalWage, CreateRegionalWageDTO, UpdateRegionalWageDTO, GetRegionalWageDTO>(unitOfWork, mapper),
            IRegionalWageService;
}