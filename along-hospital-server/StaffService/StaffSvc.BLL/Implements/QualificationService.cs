using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.QualificationDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class QualificationService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Qualification, CreateQualificationDTO, UpdateQualificationDTO, GetQualificationDTO>(
            unitOfWork, mapper), IQualificationService;
}