using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using FeedbackSvc.BLL.FilterDTOs;
using FeedbackSvc.BLL.Interfaces.Management;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace FeedbackSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.HotlineAgent))]
    public class FeedbackManagementController(IFeedbackManagementService feedbackManagementService)
        : GetController<GetFeedbackAndMedicineDTO, FeedbackFilterDTO>(feedbackManagementService);
}