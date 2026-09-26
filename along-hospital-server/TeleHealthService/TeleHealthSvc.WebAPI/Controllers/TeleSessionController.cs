using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using TeleHealthSvc.BLL.Interfaces;

namespace TeleHealthSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.TeleHealthSessionCallRolePolicy)]
    public class TeleSessionController(ITeleSessionService teleSessionService) : BaseController
    {
        private readonly ITeleSessionService _teleSessionService = teleSessionService;

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTeleSessionByTransactionIdAsync(Guid transactionId)
        {
            var teleSessionCredentialDTO = await _teleSessionService.GetTeleSessionByTransactionIdAsync(transactionId);
            return Result.SuccessData(teleSessionCredentialDTO);
        }
    }
}