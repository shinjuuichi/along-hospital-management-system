using Microsoft.AspNetCore.Authorization;
using RecruitmentSvc.BLL.DTOs.FilterDTOs;
using RecruitmentSvc.BLL.DTOs.InterviewTypeDTOs;
using RecruitmentSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace RecruitmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class InterviewTypeManagementController(IInterviewTypeService _interviewTypeService)
        : CrudController<UpsertInterviewTypeDTO, UpsertInterviewTypeDTO, GetInterviewTypeDTO, FilterInterviewTypeDTO>(_interviewTypeService)
    {
        protected override string? EntityName => "InterviewType";
    }
}