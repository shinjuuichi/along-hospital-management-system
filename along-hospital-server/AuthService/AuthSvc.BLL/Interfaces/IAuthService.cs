using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.DTOs.Request;
using AuthSvc.BLL.DTOs.Response;
using SharedLibrary.Base.Services;

namespace AuthSvc.BLL.Interfaces
{
    public interface IAuthService : IBaseCrudService<CreateAuthDTO, UpdateAuthDTO, GetAuthDTO>
    {
        Task RegisterAsync(RegisterRequestDTO request);
        Task<VerificationMethodOptionsResponseDTO> GetRegisterResendOptionsAsync(LookupVerificationMethodsRequestDTO request);
        Task<GetAuthResponseDTO> VerifyRegisterAsync(VerifyRegisterRequestDTO request);
        Task ResendRegisterAsync(ResendRegisterRequestDTO request);

        Task<GetAuthResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<GetAuthResponseDTO> GoogleLoginAsync(GoogleLoginRequestDTO request);

        Task LogoutAsync(string accessToken, string? refreshToken);

        Task<GetUserProfileDTO> GetMeAsync();

        Task UpdateAuthAccountWithUserIdAsync(UpdateAuthAccountWithUserIdDTO request);
        Task<List<int?>> GetUserIdsByFilterAsync(AuthFilterRequestDTO authFilterRequestDTO);

        Task TerminateAuthAccountsByUserIdsAsync(List<int> userIds);
        Task ChangeAuthStatusByUserIdAsync(int userId, string? status);
    }
}
