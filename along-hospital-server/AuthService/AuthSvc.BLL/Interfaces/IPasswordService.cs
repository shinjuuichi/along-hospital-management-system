using AuthSvc.BLL.DTOs.Request;
using AuthSvc.BLL.DTOs.Response;
namespace AuthSvc.BLL.Interfaces
{
    public interface IPasswordService
    {
        Task<VerificationMethodOptionsResponseDTO> GetForgotPasswordOptionsAsync(LookupVerificationMethodsRequestDTO request);
        Task ForgotPasswordRequestAsync(ForgotPasswordRequestDTO request);
        Task<VerifyForgotPasswordResponseDTO> VerifyForgotPasswordAsync(VerifyForgotPasswordRequestDTO request);
        Task ResetPasswordAsync(ResetPasswordRequestDTO request);
        Task ChangePasswordAsync(ChangePasswordRequestDTO request, string accessToken);
    }
}
