using AutoMapper;
using RecruitmentSvc.BLL.DTOs.InterviewTypeDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace RecruitmentSvc.BLL.Implements
{
    public class InterviewTypeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<InterviewType, UpsertInterviewTypeDTO, UpsertInterviewTypeDTO, GetInterviewTypeDTO>(unitOfWork, mapper),
          IInterviewTypeService;
}