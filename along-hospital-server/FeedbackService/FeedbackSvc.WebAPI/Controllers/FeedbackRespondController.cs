using FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs;
using FeedbackSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Settings;

namespace FeedbackSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.FeedbackRespondRolePolicy)]
    public class FeedbackRespondController(IFeedbackRespondService feedbackRespondService)
        : CrudController<CreateFeedbackRespondDTO, UpdateFeedbackRespondDTO, GetFeedbackRespondDTO>(feedbackRespondService)
    {
        protected override string EntityName => "FeedbackRespond";
    }
}